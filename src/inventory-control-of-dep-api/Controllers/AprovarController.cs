using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.Aprovar;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.AprovarValidators;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AprovarController : ControllerBase
    {
        private readonly IRepository<Aprovar> _aprovarRepository;
        private readonly IMapper _mapper;
        private readonly IAprovarValidator _aprovarValidator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AprovarController(IRepository<Aprovar> aprovarRepository,
            IMapper mapper, IAprovarValidator aprovarValidator, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _aprovarRepository = aprovarRepository ?? throw new ArgumentNullException(nameof(aprovarRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _aprovarValidator = aprovarValidator ?? throw new ArgumentNullException(nameof(aprovarValidator));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AprovarResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllAprovar()
        {
            try
            {
                var result = _mapper.Map<List<AprovarResponse>>(_aprovarRepository.GetAll());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAprovar([FromBody] CreateAprovarRequest request)
        {
            try
            {
                await _aprovarValidator.Validate(request.UserId, request.InventoryBookId);

                var model = _mapper.Map<Aprovar>(request);

                var result = await _aprovarRepository.Create(model);

                return CreatedAtAction(nameof(CreateAprovar), new { id = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAprovar([FromRoute] int id, [FromBody] UpdateAprovarRequest request)
        {
            try
            {
                var result = await _aprovarRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _aprovarValidator.Validate(request.UserId, request.InventoryBookId);

                var model = _mapper.Map<Aprovar>(request);

                model.Id = id;
                await _aprovarRepository.Update(model);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAprovar([FromRoute] int id)
        {
            try
            {
                var result = await _aprovarRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _aprovarRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AprovarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAprovarById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<AprovarResponse>(await _aprovarRepository.GetById(id));

                if (result is null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAprovarByInventoryBookIdId/{id}")]
        [ProducesResponseType(typeof(GetAprovarByInventoryBookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAprovarByInventoryBookIdId([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<List<AprovarResponse>>(_aprovarRepository.GetAll().ToList().Where(i => i.InventoryBookId == id).ToList());
                bool flag = true;

                foreach (var item in result)
                {
                    var user = await _userManager.FindByIdAsync(item.UserId);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (!item.IsAprove && roles.Contains("PurchaseDepartment"))
                    {
                        flag = false;
                    }

                    if (!item.IsAprove && roles.Contains("DepHead"))
                    {
                        flag = false;
                    }
                }

                var aprovar = new GetAprovarByInventoryBookResponse
                {
                    Aprovars = result,
                    IsAprove = flag
                };

                if (result is null)
                {
                    return NotFound();
                }

                return Ok(aprovar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
