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
		//public int ProjectId { get; set; }
		//public int WorkTypeId { get; set; }
		public string? TestCustomer { get; set; }
		[MaxLength(100)]
		public string? Progress { get; set; }
	}
}