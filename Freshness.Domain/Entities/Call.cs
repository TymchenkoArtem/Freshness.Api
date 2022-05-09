using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Call
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime AddedDate { get; set; }

        public bool? IsDone { get; set; }

        public DateTime? IsDoneDate { get; set; }

        public int? WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        [InverseProperty("Calls")]
        public Worker Worker { get; set; }
    }
}
