using Freshness.Common.Enums;
using System;

namespace Freshness.Models.ResponseModels
{
    public class AccessoryResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OriginalImage { get; set; }

        public string CroppedImage { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Language Language { get; set; }
    }
}
