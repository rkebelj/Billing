using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data.Entities
{
	public class Material
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int CategoryId { get; set; }
		[Required]
		[MaxLength(100)]
		public string? Name { get; set; }
		[Required]
		public int Price { get; set; }
		[Required]
		[MaxLength(100)]
		public string? Unit { get; set; }
		[MaxLength(100)]
		public string? Comment { get; set; }

	}
}
