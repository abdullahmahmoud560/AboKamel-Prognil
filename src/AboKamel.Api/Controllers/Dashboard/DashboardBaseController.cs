using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Authorization;

namespace Capsula.Api.Controllers.Dashboard;

[Route("api/dashboard/[controller]")]
[ApiController]
[Authorize] // Enforce SuperAdmin policy by default for all dashboard controllers
public class DashboardBaseController : ControllerBase
{
}
