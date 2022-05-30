using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using inventory_control_of_dep_api.Infrastructure.Services.JwtTokenServices;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_api.Models.Authorization;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators;
using IAuthorizationService = inventory_control_of_dep_api.Infrastructure.Services.AuthorizationService.IAuthorizationService;
using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserValidator _userValidator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationController(IAuthorizationService authorizationService, IJwtTokenService jwtTokenService,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserValidator userValidator)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        }

        [HttpPost("registration")]
        [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration(RegistrationRequest request)
        {
            try
            {
                await _userValidator.Validate(request.PositionId, request.FacultyId, request.DepartmentId);

                foreach (var role in request.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        return BadRequest($"Role {role} not found.");
                    }
                }

                var user = await _authorizationService.RegisterUser(request, request.Roles);

                return CreatedAtAction(nameof(Registration), _jwtTokenService.GetToken(user, request.Roles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login(LoginRequst request)
        {
            var result = await _authorizationService.LoginUser(request);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            return Forbid();
        }

        [HttpPost("logout")]
        [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authorizationService.LogoutUser();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get_roles")]
        [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
        [ProducesResponseType(typeof(RolesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetRoles()
        {
            try
            {
                var result = new RolesResponse { Roles = _roleManager.Roles.Select(r => r.Name).ToList() };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("validate_token")]
        [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ValidateToken(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok() : Forbid();
        }
    }
}