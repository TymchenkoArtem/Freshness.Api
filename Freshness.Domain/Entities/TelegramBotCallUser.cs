using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Domain.Entities
{
    public class TelegramBotCallUser
    {
        [Key]
        public long ChatId { get; set; }

        public string Phone { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string AuthorizationStage { get; set; }

        public DateTime? SignedInDate { get; set; }

        public DateTime? LastUpdate { get; set; }

    }
}
