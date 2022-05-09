using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("Customers")]
        public Address Address { get; set; }

        public DateTime AddedDate { get; set; }

        [InverseProperty("Customer")]
        public List<Note> Notes { get; set; }

        [InverseProperty("Customer")]
        public List<Order> Orders { get; set; }
    }
}
