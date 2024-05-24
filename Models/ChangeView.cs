using System.ComponentModel.DataAnnotations;
namespace InmobiliariaBaigorriaDiaz.Models
{
	public class ChangeView
	{
		[DataType(DataType.Password)]
		public string ClaveVieja { get; set; }
		[DataType(DataType.Password)]
		public string ClaveNueva { get; set; }
		[DataType(DataType.Password)]
		public string RepetirClaveNueva { get; set; }
	}
}