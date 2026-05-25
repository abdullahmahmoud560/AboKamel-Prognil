using Services.Application.Dtos.Authentication;

namespace Capsula.Application.Dtos.Authentication.Users.Customers;

public class CustomerResponseDto : BaseUserResponseDto
{
    public bool Active { get; set; }
}
