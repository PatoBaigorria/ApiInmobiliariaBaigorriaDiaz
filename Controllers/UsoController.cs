using InmobiliariaBaigorriaDiaz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBaigorriaDiaz.Controllers
{
	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsosController : ControllerBase
	{
		private readonly DataContext contexto;

		public UsosController(DataContext contexto)
		{
			this.contexto = contexto;
		}
		
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Uso>>> Get()
		{
			try
			{
				var usuario = User.Identity.Name;
				var usos = await contexto.Usodeinmueble.ToListAsync();
				return Ok(usos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}