using InmobiliariaBaigorriaDiaz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBaigorriaDiaz.Controllers
{
	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class TiposController : ControllerBase
	{
		private readonly DataContext contexto;

		public TiposController(DataContext contexto)
		{
			this.contexto = contexto;
		}
		

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Tipo>>> Get()
		{
			try
			{
				var usuario = User.Identity.Name;
				var tipos = await contexto.Tipodeinmueble.ToListAsync();
				return Ok(tipos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}