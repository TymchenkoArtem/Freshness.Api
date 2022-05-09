using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [InverseProperty("District")]
        public List<Address> Addresses { get; set; }

        [InverseProperty("Districts")]
        public List<Worker> Workers { get; set; }
    }
}
