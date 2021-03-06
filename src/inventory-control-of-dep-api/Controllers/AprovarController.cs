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
        private readonly IRepository<MaterialValue> _materialValueRepository;
        private readonly IRepository<InventoryBook> _inventoryBookRepository;
        private readonly IRepository<OperationsType> _operationsTypeRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Room> _roomRepository;

        private readonly IMapper _mapper;
        private readonly IAprovarValidator _aprovarValidator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AprovarController(IRepository<Aprovar> aprovarRepository,
            IMapper mapper, IAprovarValidator aprovarValidator, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IRepository<MaterialValue> materialValueRepository,
            IRepository<InventoryBook> inventoryBookRepository, IRepository<OperationsType> operationsTypeRepository,
            IRepository<Category> categoryRepository, IRepository<Room> roomRepository)
        {
            _aprovarRepository = aprovarRepository ?? throw new ArgumentNullException(nameof(aprovarRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _aprovarValidator = aprovarValidator ?? throw new ArgumentNullException(nameof(aprovarValidator));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

            _materialValueRepository = materialValueRepository ?? throw new ArgumentNullException(nameof(materialValueRepository));
            _inventoryBookRepository = inventoryBookRepository ?? throw new ArgumentNullException(nameof(inventoryBookRepository));
            _operationsTypeRepository = operationsTypeRepository ?? throw new ArgumentNullException(nameof(operationsTypeRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        [HttpGet("userId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<AprovarResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAprovar(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var roles = await _userManager.GetRolesAsync(user);
                var result = _mapper.Map<List<AprovarResponse>>(_aprovarRepository.GetAll()
                    .GroupBy(p => p.InventoryBookId).Select(g => g.First()).ToList());

                if (!roles.Contains("MaterialPerson"))
                {
                    result = _mapper.Map<List<AprovarResponse>>(_aprovarRepository.GetAll().Where(i => i.UserId == id).ToList());
                }

                foreach (var item in result)
                {
                    var inventoryBook = await _inventoryBookRepository.GetById(item.InventoryBookId);
                    var materialValue = await _materialValueRepository.GetById(inventoryBook.MaterialValueId);
                    var operationsType = await _operationsTypeRepository.GetById(inventoryBook.OperationTypeId);
                    var category = await _categoryRepository.GetById(materialValue.CategoryId);
                    var room = await _roomRepository.GetById(materialValue.RoomId);
                    
                    item.MaterialValueName = materialValue.Name;
                    item.OperationTypeName = operationsType.Name;
                    item.CategoryName = category.Name;
                    item.RoomNumber = room.Number;
                    if (roles.Contains("MaterialPerson"))
                    {
                        item.IsAprove = _aprovarRepository.GetAll().ToList()
                            .Where(i => i.InventoryBookId == item.InventoryBookId).All(i => i.IsAprove == true);
                    }
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

                if (result.Count() == 0)
                {
                    flag = false;
                }

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
