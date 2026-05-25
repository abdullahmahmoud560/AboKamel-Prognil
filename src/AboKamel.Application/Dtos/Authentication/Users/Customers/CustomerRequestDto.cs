using Services.Application.Dtos.Authentication;

namespace Capsula.Application.Dtos.Authentication.Users.Customers;

public class CustomerRequestDto : BaseUserRequestDto
{
    public int AreaId { get; set; }
}
