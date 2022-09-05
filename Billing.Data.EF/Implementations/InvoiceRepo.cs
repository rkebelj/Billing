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
	public class InvoiceRepo : IInvoiceRepo
	{

		private readonly InvoiceContext _context;
		private readonly IPropertyMappingService _propertyMappingService;


		public InvoiceRepo(InvoiceContext context, IPropertyMappingService propertyMappingService)
		{
			_context = context;
			_propertyMappingService = propertyMappingService ??
				throw new ArgumentNullException(nameof(propertyMappingService));
		}

		public async Task AddBillAsync(Invoice bill)
		{
			await _context.Invoice.AddAsync(bill);

		}

		public async Task<bool> BillExistsAsync(int BillId)
		{
			return await _context.Invoice.AnyAsync(l => l.Id == BillId);
		}

		public void DeleteBill(Invoice Bill)
		{
			_context.Invoice.Remove(Bill);

		}

		public async Task<PagedList<Invoice>> GetBillAsync(InvoiceResourceParameters BillParameters)
		{
			var collection = _context.Invoice
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
					_propertyMappingService.GetPropertyMapping<InvoiceModel, Invoice>();

				collection = collection.ApplySort(BillParameters.OrderBy, exercisePropertyMappingDictionary);
			}


			return await PagedList<Invoice>.Create(
				collection,
				BillParameters.PageNumber,
				BillParameters.PageSize);
		}

		public async Task<Invoice> GetBillAsync(int BillId)
		{
			return await _context.Invoice
			   .Where(l => l.Id == BillId)
			   .FirstOrDefaultAsync();
		}

		public async Task<bool> SaveAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public void UpdateBill(Invoice Bill)
		{
			//for this purpose context tracking is used
		}
	}
}
