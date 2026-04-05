using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Api.Extensions;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Operador,Entregador")]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarResumo()
        {
            var dashboard = await _dashboardService.ObterDashboardAsync(User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(dashboard);
        }
    }
}
