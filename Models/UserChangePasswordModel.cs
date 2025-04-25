using System.ComponentModel.DataAnnotations;

public class UserChangePasswordModel
{
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public string OldPassword { get; set; }

    [Required]
    public string Password { get; set; }
}
