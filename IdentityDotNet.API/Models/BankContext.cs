
using Microsoft.EntityFrameworkCore;

namespace IdentityDotNet.API.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}