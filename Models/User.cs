using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank2.Models
{
  public class User
  {
    public int Id { get; set; }

    public string? FullName { get; set; } = null!;
    [Required]
    public string? Username { get; set; } = null!;
    public string? OldPassword { get; set; } = null;
    [Required]
    public string? Password { get; set; } = null!;
    public string? Salt { get; set; } = null!;
    public string? BusinessName { get; set; } = null!;
    public string? BusinessType { get; set; } = null!;
    public string? TaxId { get; set; } = null!;
    [Required]
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? Pin { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Job { get; set; } = null!;

    [Required]
    public string? CustomerType { get; set; } = null!;// "Personal" or "Business"
    public List<Account>? Accounts { get; set; }

    public int? BranchId { get; set; }
    [ForeignKey("BranchId")]
    public Branch? Branch { get; set; }
  }
}
