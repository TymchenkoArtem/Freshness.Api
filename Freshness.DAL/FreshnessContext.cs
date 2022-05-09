using Freshness.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freshness.DAL
{
    public class FreshnessContext : DbContext
    {
        public FreshnessContext(DbContextOptions<FreshnessContext> options)
            : base(options)
        {

        }

        public DbSet<Accessory> Accessories { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Call> Calls { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderAccessory> OrderAccessories { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Street> Streets { get; set; }

        public DbSet<TelegramBotCallUser> TelegramBotCallUsers { get; set; }

        public DbSet<TelegramBotOrderUser> TelegramBotOrderUsers { get; set; }

        public DbSet<Worker> Workers { get; set; }
    }
}
