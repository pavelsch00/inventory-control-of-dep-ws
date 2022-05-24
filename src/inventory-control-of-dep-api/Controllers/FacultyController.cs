using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.JwtTokenAuth;
using inventory_control_of_dep_api.Models.Faculty;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ShouldContainAnyRole", AuthenticationSchemes = JwtAutheticationConstants.SchemeName)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IMapper _mapper;

        public FacultyController(IRepository<Faculty> facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FacultyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllFaculty()
        {
            try
            {
                var result = _mapper.Map<List<FacultyResponse>>(_facultyRepository.GetAll());

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
        public async Task<IActionResult> CreateFaculty([FromBody] CreateFacultyRequest request)
        {
            try
            {
                var model = _mapper.Map<Faculty>(request);

                var result = await _facultyRepository.Create(model);

                return CreatedAtAction(nameof(CreateFaculty), new { id = result });
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
        public async Task<IActionResult> UpdateFaculty([FromRoute] int id, [FromBody] UpdateFacultyRequest request)
        {
            try
            {
                var result = await _facultyRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<Faculty>(request);

                model.Id = id;
                await _facultyRepository.Update(model);

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
        public async Task<IActionResult> DeleteFaculty([FromRoute] int id)
        {
            try
            {
                var result = await _facultyRepository.GetById(id);

                if (result is null)
                {
                    return NotFound();
                }

                await _facultyRepository.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FacultyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FacultyResponse>> GetFacultyById([FromRoute] int id)
        {
            try
            {
                var result = _mapper.Map<FacultyResponse>(await _facultyRepository.GetById(id));

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
