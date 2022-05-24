using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.Position;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IRepository<Position> _positionRepository;
        private readonly IMapper _mapper;

        public PositionController(IRepository<Position> positionRepository, IMapper mapper)
        {
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PositionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllPosition()
        {
            try
            {
                var result = _mapper.Map<List<PositionResponse>>(_positionRepository.GetAll());

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
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequest request)
        {
            try
            {
                var model = _mapper.Map<Position>(request);

                var result = await _positionRepository.Create(model);

                return CreatedAtAction(nameof(CreatePosition), new { id = result });
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
        public async Task<IActionResult> UpdatePosition([FromRoute] int id, [FromBody] UpdatePositionRequest request)
        {
            try
            {
                var result = await _positionRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<Position>(request);

                model.Id = id;
                await _positionRepository.Update(model);

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
        public async Task<IActionResult> DeletePosition([FromRoute] int id)
        {
            try
            {
                var result = await _positionRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _positionRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PositionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PositionResponse>> GetPositionById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<PositionResponse>(await _positionRepository.GetById(id));

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
