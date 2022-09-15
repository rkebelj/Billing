using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Billing.Models.Invoice;
using Billing.Models.Material;

namespace Billing.Models.InvoiceMaterial
{
	public class InvoiceMaterialModel
	{
		public int Quantity { get; set; }


		public int InvoiceId { get; set; }
		//public InvoiceModel? Invoice { get; set; }

		public int MateriaId { get; set; }
		//public MaterialModel? Material { get; set; }



	}
}
