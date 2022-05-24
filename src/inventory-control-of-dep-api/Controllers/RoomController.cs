using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.Room;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRepository<Room> _roomRepository;
        private readonly IMapper _mapper;

        public RoomController(IRepository<Room> roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoomResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRoom()
        {
            try
            {
                var result = _mapper.Map<List<RoomResponse>>(_roomRepository.GetAll());

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
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            try
            {
                var model = _mapper.Map<Room>(request);

                var result = await _roomRepository.Create(model);

                return CreatedAtAction(nameof(CreateRoom), new { id = result });
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
        public async Task<IActionResult> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomRequest request)
        {
            try
            {
                var result = await _roomRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<Room>(request);

                model.Id = id;
                await _roomRepository.Update(model);

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
        public async Task<IActionResult> DeleteRoom([FromRoute] int id)
        {
            try
            {
                var result = await _roomRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _roomRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomResponse>> GetRoomById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<RoomResponse>(await _roomRepository.GetById(id));

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
