namespace Bank2.Models
{
    public class SharedData
    {
        public User? User { get; set; } = null!;
        public List<Account>? Account { get; set; } = null!;
        public List<Transactions>? Transactions { get; set; } = null!;
        public List<Branch> Branch { get; set; } = null!;

        // Used to send data using forms
        public string? accountType { get; set; } = null!;
        public decimal amount { get; set; }
        public int accountId { get; set; }
        public int branchId { get; set; }

        // used in payment processing
        public int fromAccount { get; set; } 
        public string? toAccount { get; set; } = null!;
        public string? transactionType { get; set; } = null!;
        public string? description { get; set; } = null!;
    }
}
