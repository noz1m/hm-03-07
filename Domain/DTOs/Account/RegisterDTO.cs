namespace Domain.DTOs.Account;

public class RegisterDTO
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
