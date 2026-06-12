using Capsula.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Capsula.Application.Dtos.Mobile.Supports;
public class SupportRequestDto : BaseRequestDto

{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IFormFile? Attachment { get; set; }
}