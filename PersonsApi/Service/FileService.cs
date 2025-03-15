namespace PersonsApi.Service
{
    public class FileService : IFileService
    {
        public string SaveFile(IFormFile file, int personId)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{personId}{fileExtension}";
            var filePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/images/{fileName}";
        }

        public void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}
