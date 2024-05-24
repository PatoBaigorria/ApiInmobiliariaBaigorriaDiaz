using InmobiliariaBaigorriaDiaz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBaigorriaDiaz.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class PagosController : ControllerBase
	{
		private readonly DataContext contexto;

		public PagosController(DataContext contexto)
		{
			this.contexto = contexto;
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Pago>> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var pagos = await contexto.Pagos
                    .Where(p => p.ContratoId == id)
                    .ToListAsync();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
	}
}
