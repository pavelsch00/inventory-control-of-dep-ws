using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.User;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IRepository<Department> _departmentRepository;

        private readonly IMapper _mapper;
        private readonly IUserValidator _userValidator;

        public UserController(UserManager<User> userManager,  RoleManager<IdentityRole> roleManager, 
            IMapper mapper, IUserValidator userValidator, IRepository<Position> positionRepository,
            IRepository<Faculty> facultyRepository, IRepository<Department> departmentRepository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));

            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
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

        [HttpPut("{email}/setIsActive")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetIsActive([FromRoute] string email, [FromBody] SetIsActiveRequest request)
        {
            try
            {
                var result = await _userManager.FindByEmailAsync(email);

                if (result is null)
                {
                    return NotFound();
                }

                result.IsActive = request.IsActive;

                await _userManager.UpdateAsync(result);

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

                var flag = await _userManager.ChangePasswordAsync(result, request.CurrentPassword, request.NewPassword);

                if (flag.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Passwords do not match");
                }
                
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

                var positions = _positionRepository.GetAll().ToList();
                var facultys = _facultyRepository.GetAll().ToList();
                var departments = _departmentRepository.GetAll().ToList();

                result.PositionName = (await _positionRepository.GetById(result.PositionId == null ? 0 : result.PositionId.Value))?.Name;
                result.FacultyName = (await _facultyRepository.GetById(result.FacultyId == null ? 0 : result.FacultyId.Value))?.Name;
                result.DepartmentName = (await _departmentRepository.GetById(result.DepartmentId == null ? 0 : result.DepartmentId.Value))?.Name;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getById/{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserById([FromRoute] string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user is null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<UserResponse>(user);

                result.Roles = await _userManager.GetRolesAsync(user);

                var positions = _positionRepository.GetAll().ToList();
                var facultys = _facultyRepository.GetAll().ToList();
                var departments = _departmentRepository.GetAll().ToList();

                result.PositionName = (await _positionRepository.GetById(result.PositionId == null ? 0 : result.PositionId.Value))?.Name;
                result.FacultyName = (await _facultyRepository.GetById(result.FacultyId == null ? 0 : result.FacultyId.Value))?.Name;
                result.DepartmentName = (await _departmentRepository.GetById(result.DepartmentId == null ? 0 : result.DepartmentId.Value))?.Name;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetAllUser()
        {
            try
            {
                var users = _userManager.Users.ToList();
                
                if (users is null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<List<UserResponse>>(users);

                var positions = _positionRepository.GetAll().ToList();
                var facultys = _facultyRepository.GetAll().ToList();
                var departments = _departmentRepository.GetAll().ToList();

                foreach (var user in result)
                {
                    user.PositionName = (await _positionRepository.GetById(user.PositionId == null ? 0 : user.PositionId.Value))?.Name;
                    user.FacultyName = (await _facultyRepository.GetById(user.FacultyId == null ? 0 : user.FacultyId.Value))?.Name;
                    user.DepartmentName = (await _departmentRepository.GetById(user.DepartmentId == null ? 0 : user.DepartmentId.Value))?.Name;
                }

                foreach (var item in result)
                {
                    item.Roles = await _userManager.GetRolesAsync(users.Where(i => i.Id == item.Id).FirstOrDefault());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}