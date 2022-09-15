using Billing.Models.InvoiceMaterial;
using Billing.Models.Material;

namespace Billing.Models.Invoice
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public  static int DDV { get; set; } = 22;
        public int? Discount { get; set; }

		public List<MaterialModel> Materials { get; set; }

	}
}