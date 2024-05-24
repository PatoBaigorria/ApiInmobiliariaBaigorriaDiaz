using System.ComponentModel.DataAnnotations;

namespace InmobiliariaBaigorriaDiaz.Models
{
	public class Tipo
	{
		[Display(Name = "ID del uso")]
		public int Id { get; set; }

		public string Nombre { get; set; } = "";
	}
}