namespace RealEstate.Application.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Moves uploaded images from the temporary upload folder to the permanent properties folder.
        /// </summary>
        Task<List<string>> MoveImagesFromTempAsync(List<string>? uploadedImagePaths);

        /// <summary>
        /// Deletes an image from the storage given its relative URL.
        /// </summary>
        Task DeleteImageAsync(string imagePath);
    }
}
