﻿namespace CookIt.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "recipe_images",
                    File = new FileDescription(fileName, ms),
                };
                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
