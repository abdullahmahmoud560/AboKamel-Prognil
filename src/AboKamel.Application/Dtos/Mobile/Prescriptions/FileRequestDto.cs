using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Dtos.Mobile.Prescriptions;

public class FileRequestDto
{
    public IFormFile File { get; set; }
}
