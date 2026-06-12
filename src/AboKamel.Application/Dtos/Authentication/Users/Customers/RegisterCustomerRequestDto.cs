using System.ComponentModel.DataAnnotations;

namespace AboKamel.Application.Dtos.Authentication.Users.Customers;

public class RegisterCustomerRequestDto
{
    [Required(ErrorMessage = "FullName is required")]
    [StringLength(200, ErrorMessage = "FullName cannot exceed 200 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "EstablishmentName is required")]
    [StringLength(200, ErrorMessage = "EstablishmentName cannot exceed 200 characters")]
    public string EstablishmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "EstablishmentType is required")]
    [StringLength(100, ErrorMessage = "EstablishmentType cannot exceed 100 characters")]
    public string EstablishmentType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmPassword is required")]
    [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string Address { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Landmark cannot exceed 200 characters")]
    public string Landmark { get; set; } = string.Empty;

    [Required(ErrorMessage = "AreaId is required")]
    public int AreaId { get; set; }
}
