using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.MaterialValue;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;
using inventory_control_of_dep_api.Infrastructure.Services.Validators.MaterialValueValidators;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MaterialValueController : ControllerBase
    {
        private readonly IRepository<MaterialValue> _materialValueRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Category> _categoryRepository;

        private readonly IMapper _mapper;
        private readonly IMaterialValueValidator _materialValueValidator;

        public MaterialValueController(IRepository<MaterialValue> materialValueRepository, 
            IMapper mapper, IMaterialValueValidator materialValueValidator,
            IRepository<Room> roomRepository, IRepository<Category> categoryRepository)
        {
            _materialValueRepository = materialValueRepository ?? throw new ArgumentNullException(nameof(materialValueRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _materialValueValidator = materialValueValidator ?? throw new ArgumentNullException(nameof(materialValueValidator));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MaterialValueResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMaterialValue()
        {
            try
            {
                var result = _mapper.Map<List<MaterialValueResponse>>(_materialValueRepository.GetAll()).Where(m => m.IsActive == false);

                foreach (var item in result)
                {
                    var room = await _roomRepository.GetById(item.RoomId);
                    var categoty = await _categoryRepository.GetById(item.CategoryId);

                    item.RoomNumber = room.Number;
                    item.CategoryName = categoty.Name;
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
        public async Task<IActionResult> CreateMaterialValue([FromBody] CreateMaterialValueRequest request)
        {
            try
            {
                await _materialValueValidator.Validate(request.CategoryId, request.RoomId);

                var model = _mapper.Map<MaterialValue>(request);
                model.DateOfIssue = model.DateOfIssue.ToUniversalTime();
                model.WriteOffDate = model.WriteOffDate.ToUniversalTime();

                var result = await _materialValueRepository.Create(model);

                return CreatedAtAction(nameof(CreateMaterialValue), new { id = result });
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
        public async Task<IActionResult> UpdateMaterialValue([FromRoute] int id, [FromBody] UpdateMaterialValueRequest request)
        {
            try
            {
                var result = await _materialValueRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _materialValueValidator.Validate(request.CategoryId, request.RoomId);

                var model = _mapper.Map<MaterialValue>(request);

                model.Id = id;
                model.DateOfIssue = model.DateOfIssue.ToUniversalTime();
                model.WriteOffDate = model.WriteOffDate.ToUniversalTime();
                model.IsActive = result.IsActive;

                await _materialValueRepository.Update(model);

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
        public async Task<IActionResult> DeleteMaterialValue([FromRoute] int id)
        {
            try
            {
                var result = await _materialValueRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _materialValueRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialValueResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MaterialValueResponse>> GetMaterialValueById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<MaterialValueResponse>(await _materialValueRepository.GetById(id));

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
    }
}
