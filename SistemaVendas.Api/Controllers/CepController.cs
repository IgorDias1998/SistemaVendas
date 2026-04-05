using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [Route("api/cep")]
    [ApiController]
    [Authorize(Roles = "Admin,Operador")]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarCepAsync(string cep)
        {
            var resultado = await _cepService.BuscarCepAsync(cep);
            return Ok(resultado);
        }
    }
}
