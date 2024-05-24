using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaBaigorriaDiaz.Models
{
	public class Inmueble
	{
		[Display(Name = "ID del inmueble")]
		public int Id { get; set; }

		[Required]
		public int PropietarioId { get; set; }

		[Required]
		public int TipoId { get; set; }

		[Required]
		public int UsoId { get; set; }

		[Required]
		public string Direccion { get; set; } = "";

		[Required]
		public int Ambientes { get; set; }

		public decimal Precio {get; set;}

		public bool Estado {get; set;}
		public string? ImagenUrl { get; set; }
		[NotMapped]
		public IFormFile Imagen { get; set;}

		[ForeignKey(nameof(PropietarioId))]
		public Propietario? Duenio { get; set; }

		[ForeignKey(nameof(TipoId))]
		public Tipo? Tipo {get;set;}

		[ForeignKey(nameof(UsoId))]
		public Uso? Uso {get;set;}
	}
}