namespace DroneApplication.FileUploadService
{
    public class LocalFileUploadService : IFileUploadService
    {

        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment environment;

        public LocalFileUploadService(Microsoft.Extensions.Hosting.IHostingEnvironment environment) { 
            this.environment = environment;
        }    
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(environment.ContentRootPath, @"wwwroot\TextFiles", file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
 