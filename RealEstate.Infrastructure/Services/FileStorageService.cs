using Microsoft.Extensions.Hosting;
using RealEstate.Application.Interfaces;

namespace RealEstate.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _wwwrootPath;

        public FileStorageService(IHostEnvironment env)
        {
            _wwwrootPath = Path.Combine(env.ContentRootPath, "wwwroot");
        }

        public async Task<List<string>> MoveImagesFromTempAsync(List<string>? uploadedImagePaths)
        {
            var resolvedImagePaths = new List<string>();

            if (uploadedImagePaths == null || uploadedImagePaths.Count == 0)
                return resolvedImagePaths;

            var tempPath = Path.Combine(_wwwrootPath, "uploads", "temp");
            var permanentPath = Path.Combine(_wwwrootPath, "uploads", "properties");

            if (!Directory.Exists(permanentPath))
                Directory.CreateDirectory(permanentPath);

            foreach (var tempUrl in uploadedImagePaths)
            {
                if (string.IsNullOrWhiteSpace(tempUrl))
                    continue;

                try
                {
                    // If the image is already in permanent folder or default images, skip moving
                    if (tempUrl.StartsWith("/uploads/properties/") || tempUrl.StartsWith("/images/"))
                    {
                        resolvedImagePaths.Add(tempUrl);
                        continue;
                    }

                    var fileName = Path.GetFileName(tempUrl);
                    var tempFile = Path.Combine(tempPath, fileName);
                    var newFile = Path.Combine(permanentPath, fileName);
                    var newUrl = "/uploads/properties/" + fileName;

                    if (File.Exists(tempFile))
                    {
                        File.Move(tempFile, newFile, overwrite: true);
                    }

                    resolvedImagePaths.Add(newUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving image {tempUrl}: {ex.Message}");
                    // Optionally log error
                }
            }

            return resolvedImagePaths;
        }

        public async Task DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            try
            {
                // Clean relative path
                var cleanedPath = imagePath.TrimStart('/')
                                           .Replace("/", Path.DirectorySeparatorChar.ToString());

                var fullPath = Path.Combine(_wwwrootPath, cleanedPath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image {imagePath}: {ex.Message}");
                // Optionally log error
            }
        }
    }
}
