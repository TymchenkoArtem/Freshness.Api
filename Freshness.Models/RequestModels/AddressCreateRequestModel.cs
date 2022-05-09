using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class AddressCreateRequestModel
    {
        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidDistrict)]
        public string District { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidStreet)]
        public string Street { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidHouse)]
        public string House { get; set; }

        [StringLength(ValidationValues.FlatMaxLength, MinimumLength = ValidationValues.FlatMinLength, ErrorMessage = ValidationMessages.InvalidFlat)]
        public string Flat { get; set; }

        [Range(ValidationValues.EntranceMinValue, ValidationValues.EntranceMaxValue, ErrorMessage = ValidationMessages.InvalidEntrance)]
        public int Entrance { get; set; }

        public override string ToString()
        {
            var address = $"{District}, вул. {Street} {House}";

            if (Flat != null && Flat != "string" && Flat != string.Empty && Entrance != 0)
            {
                address += $"/{Flat}, {Entrance} під'їзд";
            }

            return address;
        }
    }
}
