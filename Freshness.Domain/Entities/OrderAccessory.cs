using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class OrderAccessory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderAccessories")]
        public Order Order { get; set; }

        public int AccessoryId { get; set; }

        [ForeignKey("AccessoryId")]
        [InverseProperty("OrderAccessories")]
        public Accessory Accessory { get; set; }
    }
}
