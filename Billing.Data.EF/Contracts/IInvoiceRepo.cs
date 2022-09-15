using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Billing.Data.Entities;


namespace Billing.Data.EF.Contracts
{
	public interface IInvoiceRepo
	{
		Task<PagedList<Invoice>> GetInvoiceAsync(InvoiceResourceParameters invoiceParameters);
		Task<Invoice> GetInvoiceAsync(int invoice);
		Task AddInvoiceAsync(Invoice invoice);
		void UpdateInvoice(Invoice invoice);
		Task<bool> InvoiceExistsAsync(int invoiceId);
		void DeleteInvoice(Invoice invoice);
		Task<bool> SaveAsync();
	}
}
