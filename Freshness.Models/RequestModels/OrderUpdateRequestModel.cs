using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class OrderUpdateRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.IdRequired)]
        public int Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.DeliveryDateRequired)]
        public DateTime DeliveryDate { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidDeliveryTime)]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidAmount)]
        [Range(ValidationValues.AmountMinValue, ValidationValues.AmountMaxValue, ErrorMessage = ValidationMessages.InvalidAmount)]
        public int Amount { get; set; }

        public string Container { get; set; }

        public bool IsDone { get; set; }

        public int? WorkerId { get; set; }

        public string Note { get; set; }

        public CustomerUpdateRequestModel Customer { get; set; }

        public List<int> Accessories { get; set; }
    }
}
