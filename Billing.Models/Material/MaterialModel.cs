using Billing.Models.InvoiceMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Models.Material
{
	public class MaterialModel
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string? MaterialName { get; set; }
		public int Price{ get; set; }
		public string? Unit { get; set; }
		public string? Comment { get; set; }

		public IList<InvoiceMaterialModel>? InvoiceMaterials { get; set; }


	}
}
