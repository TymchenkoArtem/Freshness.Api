using Freshness.Common.Enums;
using System;

namespace Freshness.Models.ResponseModels
{
    public class ServiceResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public Language Language { get; set; }
    }
}
