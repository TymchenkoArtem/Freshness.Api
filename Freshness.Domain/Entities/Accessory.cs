using Freshness.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Accessory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string OriginalImage { get; set; }

        public string CroppedImage { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Language Language { get; set; }

        [InverseProperty("Accessory")]
        public List<OrderAccessory> OrderAccessories { get; set; }

    }
}
