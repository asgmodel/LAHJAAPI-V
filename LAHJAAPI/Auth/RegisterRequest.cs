using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Auth;

public class RegisterRequest
{
    //[JsonRequired]
    [Required(ErrorMessage = "Name field is Required")]
    public string FirsName { get; set; }
    public string LastName { get; set; }



    [Required]
    public string PhoneNumber { get; init; }

    [Required(ErrorMessage = "The Email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(50)]
    public string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>

    [Required]
    [DataType(DataType.Password)]
    [MaxLength(50)]
    public string Password { get; init; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string? Avatar { get; set; }

    [DefaultValue("https://localhost:7584/confirm-email")]
    [Required]
    public string ReturnUrl { get; set; }
}
