namespace Bank2.Models
{
    public class SharedData
    {
        public User? User { get; set; } = null!;
        public List<Account>? Account { get; set; } = null!;
        public string? accountType { get; set; } = null!;
        public List<Transactions>? Transactions { get; set; } = null!;
    }
}
