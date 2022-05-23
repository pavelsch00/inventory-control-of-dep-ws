using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.User;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAdminRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserValidator _userValidator;

        public UserController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper, IUserValidator userValidator)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        }

        [HttpPut("{email}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromRoute] string email, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(email);

                if (result is null)
                {
                    return NotFound();
                }

                await _userValidator.Validate(request.PositionId, request.FacultyId, request.DepartmentId);

                var model = _mapper.Map<User>(request);

                await _userManager.UpdateAsync(model);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{email}/changePassword")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeUserPassword([FromRoute] string email, [FromBody] ChangeUserPasswordRequest request)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(email);

                if (result is null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<User>(request);

                await _userManager.ChangePasswordAsync(model, request.NewPassword, request.NewPassword);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{email}/addRole")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUserRole([FromRoute] string email, [FromBody] AddUserRoleRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    return NotFound();
                }

                foreach (var role in request.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        return BadRequest($"Role {role} not found.");
                    }
                }

                await _userManager.AddToRolesAsync(user, request.Roles);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{email}/deleteRole")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserRole([FromRoute] string email, [FromBody] AddUserRoleRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    return NotFound();
                }

                foreach (var role in request.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        return BadRequest($"Role {role} not found.");
                    }
                }

                await _userManager.RemoveFromRolesAsync(user, request.Roles);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{email}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute] string email)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(email);

                if (result is null)
                {
                    return NotFound();
                }

                await _userManager.DeleteAsync(result);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<UserResponse>(user);
                result.Roles = await _userManager.GetRolesAsync(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}