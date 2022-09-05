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
		Task<PagedList<Invoice>> GetBillAsync(InvoiceResourceParameters BillParameters);
		Task<Invoice> GetBillAsync(int BillId);
		Task AddBillAsync(Invoice bill);
		void UpdateBill(Invoice Bill);
		Task<bool> BillExistsAsync(int BillId);
		void DeleteBill(Invoice Bill);
		Task<bool> SaveAsync();
	}
}
