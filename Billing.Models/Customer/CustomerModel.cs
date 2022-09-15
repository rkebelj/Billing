using Billing.Models.Invoice;

namespace Billing.Models
{
	public class CustomerModel
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Address { get; set; }
		public string? City{ get; set; }
		public string? PostalCode{ get; set; }
		public string? PhoneNumber{ get; set; }
		public string? Email{ get; set; }

		public IEnumerable<InvoiceModel>? Invoices { get; set; }
		public IEnumerable<InvoiceModel>? Meeting { get; set; }

	}
}
