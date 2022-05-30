using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.InventoryBook;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.InventoryBookValidators;
using Microsoft.AspNetCore.Identity;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InventoryBookController : ControllerBase
    {
        private readonly IRepository<InventoryBook> _inventoryBookRepository;
        private readonly IRepository<MaterialValue> _materialValueRepository;
        private readonly IRepository<OperationsType> _operationsTypeRepository;
        private readonly IRepository<Aprovar> _aprovarRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMapper _mapper;
        private readonly IInventoryBookValidator _inventoryBookValidator;
        public InventoryBookController(IRepository<InventoryBook> inventoryBookRepository,
            IMapper mapper, IInventoryBookValidator inventoryBookValidator, IRepository<OperationsType> operationsTypeRepository,
            IRepository<MaterialValue> materialValueRepository, IRepository<Aprovar> aprovarRepository, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _operationsTypeRepository = operationsTypeRepository ?? throw new ArgumentNullException(nameof(operationsTypeRepository));
            _materialValueRepository = materialValueRepository ?? throw new ArgumentNullException(nameof(materialValueRepository));
            _inventoryBookRepository = inventoryBookRepository ?? throw new ArgumentNullException(nameof(inventoryBookRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _inventoryBookValidator = inventoryBookValidator ?? throw new ArgumentNullException(nameof(inventoryBookValidator));
            _aprovarRepository = aprovarRepository ?? throw new ArgumentNullException(nameof(aprovarRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryBookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllInventoryBook()
        {
            try
            {
                var result = _mapper.Map<List<InventoryBookResponse>>(_inventoryBookRepository.GetAll());

                foreach (var item in result)
                {
                    var materialValue = await _materialValueRepository.GetById(item.MaterialValueId);
                    var operationsType = await _operationsTypeRepository.GetById(item.OperationTypeId);
                    item.MaterialValueName = materialValue.Name;
                    item.MaterialValuInventoryNumber = materialValue.InventoryNumber;
                    item.OperationTypeName = operationsType.Name;
                }

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
        public async Task<IActionResult> CreateInventoryBook([FromBody] CreateInventoryBookRequest request)
        {
            try
            {
                await _inventoryBookValidator.Validate(request.UserId,
                    request.OperationTypeId, request.MaterialValueId);

                var model = _mapper.Map<InventoryBook>(request);

                var result = await _inventoryBookRepository.Create(model);

                var user = _userManager.Users.ToList();
                var aproval = new Aprovar();
                foreach (var item in user)
                {
                    var roles = await _userManager.GetRolesAsync(item);
                    if (roles.Contains("DepHead") || roles.Contains("PurchaseDepartment"))
                    {
                        aproval = new Aprovar
                        {
                            InventoryBookId = result,
                            IsAprove = false,
                            UserId = item.Id
                        };

                        await _aprovarRepository.Create(aproval);
                    } 
                }



                return CreatedAtAction(nameof(CreateInventoryBook), new { id = result });
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
        public async Task<IActionResult> UpdateInventoryBook([FromRoute] int id, [FromBody] UpdateInventoryBookRequest request)
        {
            try
            {
                var result = await _inventoryBookRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _inventoryBookValidator.Validate(request.UserId,
                    request.OperationTypeId, request.MaterialValueId);

                var model = _mapper.Map<InventoryBook>(request);

                model.Id = id;
                await _inventoryBookRepository.Update(model);

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
        public async Task<IActionResult> DeleteInventoryBook([FromRoute] int id)
        {
            try
            {
                var result = await _inventoryBookRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _inventoryBookRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InventoryBookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InventoryBookResponse>> GetInventoryBookById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<InventoryBookResponse>(await _inventoryBookRepository.GetById(id));

                if (result is null)
                {
                    return NotFound();
                }

                var materialValue = await _materialValueRepository.GetById(result.MaterialValueId);
                var operationsType = await _operationsTypeRepository.GetById(result.OperationTypeId);
                result.MaterialValueName = materialValue.Name;
                result.MaterialValuInventoryNumber = materialValue.InventoryNumber;
                result.OperationTypeName = operationsType.Name;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
