using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime AddedDate { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Notes")]
        public Customer Customer { get; set; }
    }
}
