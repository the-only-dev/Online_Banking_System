using System.ComponentModel.DataAnnotations.Schema;

namespace Bank2.Models
{
    public class Account
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public string? AccountNo { get; set; } = null!;
        public string? AccountType { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }
        public string? AccountStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<Transactions>? Transactions { get; set; }
    }
}
