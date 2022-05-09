using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string DeliveryTime { get; set; }

        public DateTime AddedDate { get; set; }

        public int Amount { get; set; }

        public string Container { get; set; }

        public int TotalCost { get; set; }

        public bool IsDone { get; set; }

        public string Note { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public Customer Customer { get; set; }

        [InverseProperty("Order")]
        public List<OrderAccessory> OrderAccessories { get; set; }

        public DateTime? IsDoneDate { get; set; }

        public int? WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        [InverseProperty("Orders")]
        public Worker Worker { get; set; }
    }
}
