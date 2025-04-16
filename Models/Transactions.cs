using System.ComponentModel.DataAnnotations.Schema;

namespace Bank2.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
        public string? AccountNumber { get; set; } = null!;
        public string? Type { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string? Description { get; set; } = null!;

    }
}
