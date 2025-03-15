using PersonsApi.Models;

namespace PersonsApi.Repository
{
    public interface IPersonRepository
    {
        Task<Person?> GetPersonByIdAsync(int id);
        Task<IEnumerable<Person>> GetPeopleAsync(string? search, string? firstName, string? lastName, string? personalNumber, int? page, int? pageSize);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
        Task AddRelatedIndividualAsync(int personId, RelatedIndividual relatedIndividual);
        Task DeleteRelatedIndividualAsync(int personId, int relatedIndividualId);
        Task UpdatePersonImageAsync(int personId, IFormFile file);
    }
}
