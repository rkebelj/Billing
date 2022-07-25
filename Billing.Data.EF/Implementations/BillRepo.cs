using Billing.Business.Contracts;
using Billing.Data;
using Billing.Data.EF.Contracts;
using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using Billing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data.EF.Implementations
{
	public class BillRepo : IBillRepo
	{

		private readonly BillContext _context;
		private readonly IPropertyMappingService _propertyMappingService;


		public BillRepo(BillContext context, IPropertyMappingService propertyMappingService)
		{
			_context = context;
			_propertyMappingService = propertyMappingService ??
				throw new ArgumentNullException(nameof(propertyMappingService));
		}

		public async Task AddBillAsync(Bill bill)
		{
			await _context.Bill.AddAsync(bill);

		}

		public async Task<bool> BillExistsAsync(int BillId)
		{
			return await _context.Bill.AnyAsync(l => l.Id == BillId);
		}

		public void DeleteBill(Bill Bill)
		{
			_context.Bill.Remove(Bill);

		}

		public async Task<PagedList<Bill>> GetBillAsync(BillResourceParameters BillParameters)
		{
			var collection = _context.Bill
			   .OrderBy(a => a.Id)
			   .AsQueryable();

			if (!string.IsNullOrEmpty(BillParameters.SearchQuery))
			{
				var searchQuery = BillParameters.SearchQuery
					.Trim().ToLower();

				collection = collection
					.Where(a => a.Id.ToString().Contains(searchQuery));
			}

			if (!string.IsNullOrWhiteSpace(BillParameters.OrderBy))
			{
				var exercisePropertyMappingDictionary =
					_propertyMappingService.GetPropertyMapping<BillModel, Bill>();

				collection = collection.ApplySort(BillParameters.OrderBy, exercisePropertyMappingDictionary);
			}


			return await PagedList<Bill>.Create(
				collection,
				BillParameters.PageNumber,
				BillParameters.PageSize);
		}

		public async Task<Bill> GetBillAsync(int BillId)
		{
			return await _context.Bill
			   .Where(l => l.Id == BillId)
			   .FirstOrDefaultAsync();
		}

		public async Task<bool> SaveAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public void UpdateBill(Bill Bill)
		{
			//for this purpose context tracking is used
		}
	}
}
