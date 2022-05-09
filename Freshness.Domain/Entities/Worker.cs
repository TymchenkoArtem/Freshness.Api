using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freshness.Domain.Entities
{
    public class Worker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpireDate { get; set; }

        public DateTime AddedDate { get; set; }

        [InverseProperty("Workers")]
        public List<District> Districts { get; set; }

        [InverseProperty("Worker")]
        public List<Call> Calls { get; set; }

        [InverseProperty("Worker")]
        public List<Order> Orders { get; set; }
    }
}
