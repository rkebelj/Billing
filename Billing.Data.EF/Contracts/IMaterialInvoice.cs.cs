using Billing.Data.Entities;
using Billing.Helpers.ResourceParameters;
using Billing.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data.EF.Contracts
{
	internal interface IMaterialInvoiceRepo
	{
		Task<PagedList<Invoice>> GetMaterialsOfInvoiceAsync(InvoiceResourceParameters invoiceParameters);
		Task<Invoice> GetMaterialsOfInvoicesAsync(int invoice);
		Task AddMaterialsOfInvoiceAsync(Invoice invoice);
		void UpdateMaterialsOfInvoice(Invoice invoice);
		Task<bool> MaterialsOfInvoiceExistsAsync(int invoiceId);
		void DeleteInvoice(Invoice invoice);
		Task<bool> SaveAsync();
	}
}
