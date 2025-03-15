namespace PersonsApi.Service
{
    public interface IFileService
    {
        string SaveFile(IFormFile file, int personId);
        void DeleteFile(string filePath);
    }
}
