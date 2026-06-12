using Services.Application.Dtos.Authentication;

namespace Capsula.Application.Dtos.Authentication.Users.Customers;

public class CustomerResponseDto : BaseUserResponseDto
{
    public bool Active { get; set; }
    public string EstablishmentType { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Landmark { get; set; } = string.Empty;
    public int AreaId { get; set; }
}
