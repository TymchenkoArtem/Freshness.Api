using Freshness.Models.Models;
using Microsoft.AspNetCore.Http;

namespace Freshness.Services.Interfaces
{
    public interface IImageProcessor
    {
        ImageModel Upload(IFormFile file, string folder = "images");
    }
}
