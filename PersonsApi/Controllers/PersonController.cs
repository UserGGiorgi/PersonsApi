using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonsApi.Dtos;
using PersonsApi.DTOs;
using PersonsApi.Models;
using PersonsApi.Repository;

namespace PersonsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonController(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonResponseDto>> GetPersonById(int id)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            var personResponseDto = _mapper.Map<PersonResponseDto>(person);
            return Ok(personResponseDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople(
            [FromQuery] string? search,
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? personalNumber,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10)
        {
            var people = await _personRepository.GetPeopleAsync(search, firstName, lastName, personalNumber, page, pageSize);
            var peopleDto = _mapper.Map<IEnumerable<PersonDTO>>(people);
            return Ok(peopleDto);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson(PersonRequestDTO personRequestDto)
        {
            var person = _mapper.Map<Person>(personRequestDto);
            await _personRepository.AddPersonAsync(person);
            var personResponseDto = _mapper.Map<PersonResponseDto>(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, personResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonRequestDTO personRequestDto)
        {
            var existingPerson = await _personRepository.GetPersonByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            _mapper.Map(personRequestDto, existingPerson);

            await _personRepository.UpdatePersonAsync(existingPerson);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            await _personRepository.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/related-individuals")]
        public async Task<IActionResult> AddRelatedIndividual(int id, RelatedIndividualDTO relatedIndividualDto)
        {
            var relatedIndividual = _mapper.Map<RelatedIndividual>(relatedIndividualDto);
            await _personRepository.AddRelatedIndividualAsync(id, relatedIndividual);
            return NoContent();
        }

        [HttpDelete("{id}/related-individuals/{relatedIndividualId}")]
        public async Task<IActionResult> DeleteRelatedIndividual(int id, int relatedIndividualId)
        {
            await _personRepository.DeleteRelatedIndividualAsync(id, relatedIndividualId);
            return NoContent();
        }

        [HttpPost("{PersonId}/upload-image")]
        public async Task<IActionResult> UploadImage(int PersonId, IFormFile file)
        {
            try
            {
                await _personRepository.UpdatePersonImageAsync(PersonId, file);
                return Ok(new { Message = "Image uploaded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
