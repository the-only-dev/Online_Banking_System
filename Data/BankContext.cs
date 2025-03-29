using Bank2.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank2.Data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //        new User { Id = 1, Username = "Admin", Password = "Admin", BranchId = 1, Phone = "0000000000", FullName = "Admin", Address = "Admin", Email = "Admin@gmail.com", BusinessName = "Admin", BusinessType = "Admin", TaxId = "Admin", Pin = "Admin", Job = "Admin", CustomerType = "Admin" }
        //        );
        //    modelBuilder.Entity<Account>().HasData(
        //        new Account {Id=1, AccountNo="Acc10000801",AccountStatus="Active",AccountType="Savings",AccountBalance=453.32m ,UserId=8 }
        //        );
        //    modelBuilder.Entity<Branch>().HasData(
        //        new Branch { Id=1, IFSCCode="1234AGFC", Name="First"},
        //        new Branch { Id=2, IFSCCode="5678DEFG", Name="Second"}
        //        );
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
