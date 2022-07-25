using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data.EF.Contracts
{
	public interface IBillRepo
	{
		Task<PagedList<Bill>> GetBillAsync(BillResourceParameters BillParameters);
		Task<Bill> GetBillAsync(int BillId);
		Task AddBillAsync(Bill bill);
		void UpdateBill(Bill Bill);
		Task<bool> BillExistsAsync(int BillId);
		void DeleteBill(Bill Bill);
		Task<bool> SaveAsync();
	}
}
