using System;

namespace Freshness.Models.ResponseModels
{
    public class AddressResponseModel
    {
        public int Id { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public int? Entrance { get; set; }

        public override string ToString()
        {
            return District;

            var address = $"{District}, вул. {Street} {House}";

            if (Flat != null && Flat != "string" && Flat != string.Empty && Entrance != 0)
            {
                address += $"/{Flat}, {Entrance} під'їзд";
            }

            return address;
        }
    }
}
