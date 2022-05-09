using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        [InverseProperty("Addresses")]
        public District District { get; set; }

        public int StreetId { get; set; }

        [ForeignKey("StreetId")]
        [InverseProperty("Addresses")]
        public Street Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public int? Entrance { get; set; }

        [InverseProperty("Address")]
        public List<Customer> Customers { get; set; }

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
