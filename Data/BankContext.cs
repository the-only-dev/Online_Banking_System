using Bank2.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank2.Data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var account = modelBuilder.Entity<Account>();
            var user = modelBuilder.Entity<User>();
            var branch = modelBuilder.Entity<Branch>();
            var transaction = modelBuilder.Entity<Transactions>();

            branch.HasIndex(b => b.IFSCCode).IsUnique();

            user.HasIndex(u => u.Username).IsUnique();
            user.HasIndex(u => u.Phone).IsUnique();
            user.HasIndex(u => u.Email).IsUnique();
            user.HasIndex(u => u.TaxId).IsUnique();

            account.HasIndex(ac => ac.AccountNo).IsUnique();

            user.HasData(
                new User {  
                  Id = 1, Username = "Admin", Password = "Admin", 
                  BranchId = 1, Phone = "0000000000", FullName = "Admin", 
                  Address = "Admin", Email = "Admin@gmail.com", BusinessName = "Admin", 
                  CustomerType = "Admin" }
                );

            account.HasData(
                new Account {   
                  Id = 1,
                  UserId = 1, AccountNo = "A/C-0001", AccountStatus = "Active", 
                  AccountType = "Savings", AccountBalance = 45403.32m},

                new Account {   
                  Id = 2,
                  UserId = 1, AccountNo = "A/C-0002", AccountStatus = "Active", 
                  AccountType = "Savings", AccountBalance = 503.32m}
                );

            transaction.HasData(
                new Transactions {  
                  Id=1, AccountId=1, AccountNumber="A/C-0001", Type="Credit", 
                  Amount=500.00m, Balance= 45403.32m, Description="Sent 500"},

                new Transactions {  
                  Id=2, AccountId=1, AccountNumber="A/C-0002", Type="Debit", 
                  Amount=500.00m, Balance=503.32m, Description="Sent 500"}
                );

            branch.HasData(
                new Branch { 
                  Id = 1, IFSCCode = "M46001BZU", Name = "Main", 
                  Pin = "460001", Phone = "8978887349"},

                new Branch { 
                  Id = 2, IFSCCode = "C46001BZU", Name = "City", 
                  Pin = "460001", Phone = "9856382368"}
                );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
