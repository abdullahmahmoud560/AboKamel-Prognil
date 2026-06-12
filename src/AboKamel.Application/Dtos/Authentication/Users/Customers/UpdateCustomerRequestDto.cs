using System.ComponentModel.DataAnnotations;

namespace AboKamel.Application.Dtos.Authentication.Users.Customers;

public class UpdateCustomerRequestDto
{
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(200)]
    public string Landmark { get; set; } = string.Empty;

    [StringLength(200)]
    public string EstablishmentName { get; set; } = string.Empty;

    [StringLength(100)]
    public string EstablishmentType { get; set; } = string.Empty;

    public int AreaId { get; set; }
}
