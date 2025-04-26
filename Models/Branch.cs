using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank2.Models
{
  public class Branch
  {
    public int Id { get; set; }
    public string? IFSCCode { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? Pin { get; set; }
    public string? Phone { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalAccounts { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<User>? Users { get; set; }

  }
}
