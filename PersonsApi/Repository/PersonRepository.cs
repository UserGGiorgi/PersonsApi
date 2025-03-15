using Microsoft.EntityFrameworkCore;
using PersonsApi.Data;
using PersonsApi.Models;
using PersonsApi.Service;

namespace PersonsApi.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public PersonRepository(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return await _context.People
                .Include(p => p.City)
                .Include(p => p.PhoneNumbers)
                .Include(p => p.RelatedIndividuals)
                .ThenInclude(ri => ri.RelatedPerson)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync(string? search, string? firstName, string? lastName, string? personalNumber, int? page, int? pageSize)
        {
            var query = _context.People
                   .Include(p => p.City)
                   .Include(p => p.PhoneNumbers)
                   .Include(p => p.RelatedIndividuals)
                   .ThenInclude(ri => ri.RelatedPerson)
                   .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.FirstName.Contains(search) ||
                    p.LastName.Contains(search) ||
                    p.PersonalNumber.Contains(search));
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(p => p.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(p => p.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(personalNumber))
            {
                query = query.Where(p => p.PersonalNumber.Contains(personalNumber));
            }

            int skip = ((page ?? 1) - 1) * (pageSize ?? 10);
            int take = pageSize ?? 10;

            return await query.Skip(skip).Take(take).ToListAsync();
        }
        public async Task AddPersonAsync(Person person)
        {
            var phoneNumbers = person.PhoneNumbers.ToList();
            person.PhoneNumbers = new List<PhoneNumber>();

            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            foreach (var phoneNumber in phoneNumbers)
            {
                phoneNumber.PersonId = person.Id;
                _context.PhoneNumbers.Add(phoneNumber);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            _context.People.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            var person = await _context.People
                .Include(p => p.RelatedIndividuals)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {id} not found.");
            }

            var relatedIndividuals = await _context.RelatedIndividuals
                .Where(ri => ri.RelatedPersonId == id)
                .ToListAsync();

            foreach (var relatedIndividual in relatedIndividuals)
            {
                await DeleteRelatedIndividualAsync(relatedIndividual.PersonId, relatedIndividual.RelatedPersonId);
            }
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }

        public async Task AddRelatedIndividualAsync(int personId, RelatedIndividual relatedIndividual)
        {
            if (relatedIndividual == null)
            {
                throw new ArgumentNullException(nameof(relatedIndividual));
            }

            if (personId == relatedIndividual.RelatedPersonId)
            {
                throw new InvalidOperationException("A person cannot be related to themselves.");
            }

            var person = await GetPersonByIdAsync(personId);
            var relatedPerson = await GetPersonByIdAsync(relatedIndividual.RelatedPersonId);
            ArgumentNullException.ThrowIfNull(person);

            var existingRelationship = await _context.RelatedIndividuals
                   .FirstOrDefaultAsync(ri =>
                   ri.PersonId == personId &&
                   ri.RelatedPersonId == relatedIndividual.RelatedPersonId);

            if (existingRelationship != null)
            {
                await DeleteRelatedIndividualAsync(personId, relatedIndividual.RelatedPersonId);
            }
            relatedIndividual.PersonId = personId;
            person.RelatedIndividuals.Add(relatedIndividual);
            AddReverseRelationship(person, relatedIndividual);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRelatedIndividualAsync(int personId, int relatedIndividualId)
        {
            var relatedIndividual = await GetRelatedIndividualAsync(personId, relatedIndividualId);

            var relatedPersonId = relatedIndividual.RelatedPersonId;

            var person = await GetPersonByIdAsync(personId);
            if (person != null)
            {
                person.RelatedIndividuals.Remove(relatedIndividual);
            }
            var reverseRelationship = await _context.RelatedIndividuals
                .FirstOrDefaultAsync(ri =>
                    ri.PersonId == relatedPersonId &&
                    ri.RelatedPersonId == personId);

            if (reverseRelationship != null)
            {
                _context.RelatedIndividuals.Remove(reverseRelationship);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePersonImageAsync(int personId, IFormFile file)
        {
            var person = await _context.People.FindAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            ValidateFile(file);
            if (person.Image != null)
            { 
              _fileService.DeleteFile(person.Image); 
            }

            var newImagePath = _fileService.SaveFile(file, personId);
            person.Image = newImagePath;

            await _context.SaveChangesAsync();
        }
        private async Task<RelatedIndividual> GetRelatedIndividualAsync(int personId, int relatedIndividualId)
        {
            var person = await _context.People
                .Include(p => p.RelatedIndividuals)
                .ThenInclude(ri => ri.RelatedPerson)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {personId} not found.");
            }

            var relatedIndividual = person.RelatedIndividuals
                .FirstOrDefault(ri => ri.RelatedPersonId == relatedIndividualId);

            if (relatedIndividual == null)
            {
                throw new KeyNotFoundException($"Related individual with ID {relatedIndividualId} not found.");
            }

            return relatedIndividual;
        }

        private void AddReverseRelationship(Person person, RelatedIndividual relatedIndividual)
        {
            var reverseRelationship = new RelatedIndividual
            {
                PersonId = relatedIndividual.RelatedPersonId,
                RelatedPersonId = person.Id,
                RelationshipType = relatedIndividual.RelationshipType
            };

            var relatedPerson = _context.People.Find(relatedIndividual.RelatedPersonId);
            relatedPerson?.RelatedIndividuals.Add(reverseRelationship);
        }
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/svg+xml" };
            if (!allowedContentTypes.Contains(file.ContentType))
            {
                throw new ArgumentException("Invalid file type. Only image files are allowed.");
            }
        }
    }
}
