namespace Bank2.Models
{
  public class ProfileUpdate
  {
    public string? FullName { get; set; } = null!;
    public string? Username { get; set; } = null!;
    public string? OldPassword { get; set; } = null!;
    public string? Password { get; set; } = null!;
    public string? BusinessName { get; set; } = null!;
    public string? BusinessType { get; set; } = null!;
    public string? TaxId { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? Pin { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Job { get; set; } = null!;
  }
}
