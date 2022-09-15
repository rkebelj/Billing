using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Data.Entities

{
	public class Invoice
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int CustomerId { get; set; }
		//public int WorkTypeId { get; set; }
		[Required]
		//[DataType(DataType.Date)]
		//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? Date{ get; set; }
		[Required]
		[MaxLength(100)]
		public static int DDV { get; set; } = 22;
		public int? Discount { get; set; }

	}
}