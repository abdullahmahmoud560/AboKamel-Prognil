using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.Api.Controllers.Mobile;

[Route("api/mobile/[controller]")]
[ApiController]
[Authorize]
public class MobileBaseController : ControllerBase
{
}
