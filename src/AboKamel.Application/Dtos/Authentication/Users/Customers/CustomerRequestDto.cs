using Services.Application.Dtos.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Capsula.Application.Dtos.Authentication.Users.Customers;

public class CustomerRequestDto : BaseUserRequestDto
{
    public int AreaId { get; set; }
    
    [Required]
    public string EstablishmentType { get; set; } = string.Empty;
}
