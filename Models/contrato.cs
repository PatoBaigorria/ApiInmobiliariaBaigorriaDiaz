using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaBaigorriaDiaz.Models
{
	public class Contrato
	{
		[Display(Name = "ID del contrato")]
		public int Id { get; set; }

		[Required]
		public int InquilinoId { get; set; }

		[Required]
		public int InmuebleId { get; set; }

		[Required]
		public decimal Precio { get; set; }

		[Required]
		public DateOnly FechaInicio { get; set; }

		[Required]
		public DateOnly FechaFin { get; set; }

		[ForeignKey(nameof(InquilinoId))]
		public Inquilino? Inquilino { get; set; }

		[ForeignKey(nameof(InmuebleId))]
		public Inmueble? Inmueble { get; set; }
	}
}