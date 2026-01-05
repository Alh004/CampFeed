using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CampWebservice.Configuration;
using Microsoft.Extensions.Options;
f
namespace CampWebservice.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> settings)
        {
            var account = new Account(
                settings.Value.CloudName,
                settings.Value.ApiKey,
                settings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "campfeed/issues"
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}   