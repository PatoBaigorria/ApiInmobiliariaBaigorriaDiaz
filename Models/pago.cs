using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaBaigorriaDiaz.Models
{
	public class Pago
	{
		[Display(Name = "ID del pago")]
		public int Id { get; set; }

		[Required]
		public int ContratoId { get; set; }

		[Required]
		public decimal Monto { get; set; }

		[Required]
		public int NumeroDePago {get; set;}

		[Required]
		public DateOnly Fecha { get; set; }

		[ForeignKey(nameof(ContratoId))]
		[NotMapped]
		public Contrato? Contrato { get; set; }
	}
}