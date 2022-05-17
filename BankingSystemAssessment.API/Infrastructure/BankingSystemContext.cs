using BankingSystemAssessment.API.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAssessment.API.Infrastructure
{
    public partial class BankingSystemContext : DbContext
    {
        public BankingSystemContext()
        {
        }

        public BankingSystemContext(DbContextOptions<BankingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}