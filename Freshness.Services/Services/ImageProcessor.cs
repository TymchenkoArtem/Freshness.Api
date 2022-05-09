using Freshness.Common.CustomExceptions;
using Freshness.Common.ResponseMessages;
using Freshness.Models.Models;
using Freshness.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Freshness.Services.Services
{
    public class ImageProcessor : IImageProcessor
    {
        private const int MaxSize = 5242880;

        private readonly List<string> AllowedExtensions = new List<string>()
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        private const int MinWidth = 400;
        private const int MinHeight = 500;

        private const int MaxWidth = 5000;
        private const int MaxHeight = 5000;

        private const int CroppedHeight = 750;
        private const int CroppedWidth = 600;

        private void ValidateWeightAndExtension(IFormFile file)
        {
            if (file.Length > MaxSize)
            {
                throw new CustomException(ValidationMessages.InvalidImageWeight);
            }

            var imageExtension = Path.GetExtension(file.FileName);

            if (!AllowedExtensions.Any(item => item == imageExtension.ToLower()))
            {
                throw new CustomException(ValidationMessages.InvalidImageExtension);
            }
        }

        private void ValidateSize(Image image)
        {
            if (image.Width < MinWidth || image.Height < MinHeight || image.Width > MaxWidth || image.Height > MaxHeight)
            {
                throw new CustomException(ValidationMessages.InvalidImageSize);
            }
        }

        private bool Resize(Image image)
        {
            if ((image.Width == CroppedWidth) && (image.Height == CroppedHeight))
            {
                return false;
            }

            image.Mutate(x => x.Resize(CroppedWidth, CroppedHeight));

            return true;
        }

        private string GeneratePath(string folder, string extension)
        {
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            while (File.Exists(Path.Combine(folder, fileName)))
            {
                fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            }

            var filePath = Path.Combine(folder, $"{fileName}.{extension}");

            return filePath;
        }

        private void CheckDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        private Image GetImageFromFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var image = Image.Load(stream);

            return image;
        }

        private ImageModel SaveImageToFolderInTwoCopies(Image image, string folder, string extension)
        {
            var filePath = GeneratePath(folder, extension);

            image.Save(filePath);

            var imageRequestModel = new ImageModel()
            {
                OriginalImage = filePath
            };

            if (Resize(image))
            {
                filePath = GeneratePath(folder, extension);

                image.Save(filePath);
            }

            imageRequestModel.CroppedImage = filePath;

            return imageRequestModel;
        }

        public ImageModel Upload(IFormFile file, string folder)
        {
            CheckDirectory(folder);

            ValidateWeightAndExtension(file);

            var image = GetImageFromFile(file);

            ValidateSize(image);

            var imageExtension = Path.GetExtension(file.FileName);

            var imageRequestModel = SaveImageToFolderInTwoCopies(image, folder, imageExtension);

            return imageRequestModel;
        }
    }
}
