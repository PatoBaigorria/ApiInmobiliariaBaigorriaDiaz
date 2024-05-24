using Microsoft.EntityFrameworkCore;
using InmobiliariaBaigorriaDiaz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace InmobiliariaBaigorriaDiaz.Controllers
{
	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class InmueblesController : Controller
	{
		private readonly DataContext contexto;
		private readonly IWebHostEnvironment environment;

		public InmueblesController(DataContext contexto, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.environment = environment;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var usuario = User.Identity.Name;
				return Ok(contexto.Inmuebles
				.Include(e => e.Duenio)
				.Include(t => t.Tipo)
				.Include(u => u.Uso)
				.Where(e => e.Duenio.Email == usuario));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromForm] Inmueble inmueble)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var propietarioId = contexto.Propietarios.Single(e => e.Email == User.Identity.Name).Id;
					inmueble.PropietarioId = propietarioId; // Asegúrate de asignar el propietario al inmueble

					contexto.Inmuebles.Add(inmueble);
					await contexto.SaveChangesAsync(); // Guarda el inmueble primero

					if (inmueble.Imagen != null)
					{
						var resizedImagePath = await ProcesarImagenAsync(inmueble);
						if (resizedImagePath == null)
						{
							return BadRequest("Formato de imagen no válido.");
						}

						inmueble.ImagenUrl = resizedImagePath;
						//contexto.Inmuebles.Update(inmueble); // Actualiza el inmueble con la URL de la imagen
						await contexto.SaveChangesAsync(); // Guarda los cambios
					}

					return CreatedAtAction(nameof(Get), new { id = inmueble.Id }, inmueble);
				}

				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException?.Message ?? ex.Message);
			}
		}

		private async Task<string?> ProcesarImagenAsync(Inmueble inmueble)
		{
			string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".jfif", ".bmp" };
			var extension = Path.GetExtension(inmueble.Imagen.FileName).ToLower();
			if (!allowedExtensions.Contains(extension))
			{
				return "Formato de imagen no válido.";
			}

			var directoryPath = Path.Combine(environment.WebRootPath, "uploads");
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			// Renombrar el archivo y obtener su nueva ruta
			var inmuebleFileName = $"inmueble_{inmueble.PropietarioId}_{inmueble.Id}{extension}";
			var inmuebleFilePath = Path.Combine(directoryPath, inmuebleFileName);

			// Guardar el archivo en el directorio 'uploads'
			using (var stream = new FileStream(inmuebleFilePath, FileMode.Create))
			{
				await inmueble.Imagen.CopyToAsync(stream);
			}

			// Redimensionar la imagen antes de guardarla
			var resizedImagePath = ResizeImage(inmuebleFilePath);

			// Reemplazar separadores de directorios incorrectos por correctos para URLs
			var urlPath = resizedImagePath.Replace("\\", "/");

			// Retorna la ruta de la imagen redimensionada con separadores correctos
			return urlPath;
		}

		private string ResizeImage(string imagePath)
		{
			using (var image = Image.Load(imagePath))
			{
				image.Mutate(x => x.Resize(500, 500));
				var resizedImagePath = Path.Combine(environment.WebRootPath, "uploads", Path.GetFileName(imagePath));
				image.Save(resizedImagePath);
				return Path.Combine("uploads", Path.GetFileName(imagePath)).Replace("\\", "/");
			}
		}

		[HttpPut("cambiologico/{id}")]
		public async Task<IActionResult> CambioLogico(int id)
		{
			try
			{
				var usuario = User.Identity.Name;
				var entidad = contexto.Inmuebles.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == usuario);
				if (entidad != null)
				{
					Console.WriteLine(entidad.Estado);
					if(entidad.Estado){
						entidad.Estado = false;
					} else {
						entidad.Estado = true;
					}
					contexto.Inmuebles.Update(entidad);
					contexto.SaveChanges();
					return Ok("Cambio efectuado con exito");
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}