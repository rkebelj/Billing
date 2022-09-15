using Billing.Business.Contracts;
using Billing.Data;
using Billing.Data.EF.Contracts;
using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using Billing.Models;
using Billing.Models.Invoice;
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

		private readonly BillingContext _context;
		private readonly IPropertyMappingService _propertyMappingService;


		public InvoiceRepo(BillingContext context, IPropertyMappingService propertyMappingService)
		{
			_context = context;
			_propertyMappingService = propertyMappingService ??
				throw new ArgumentNullException(nameof(propertyMappingService));
		}

		public async Task AddInvoiceAsync(Invoice invoice)
		{
			await _context.Invoice.AddAsync(invoice);

		}

		public async Task<bool> InvoiceExistsAsync(int invoiceId)
		{
			return await _context.Invoice.AnyAsync(l => l.Id == invoiceId);
		}

		public void DeleteInvoice(Invoice invoice)
		{
			_context.Invoice.Remove(invoice);

		}

		public async Task<PagedList<Invoice>> GetInvoiceAsync(InvoiceResourceParameters invoiceParameters)
		{
			var collection = _context.Invoice
			   .OrderBy(a => a.Id)
			   .AsQueryable();

			if (!string.IsNullOrEmpty(invoiceParameters.SearchQuery))
			{
				var searchQuery = invoiceParameters.SearchQuery
					.Trim().ToLower();

				collection = collection
					.Where(a => a.Id.ToString().Contains(searchQuery));
			}

			if (!string.IsNullOrWhiteSpace(invoiceParameters.OrderBy))
			{
				var exercisePropertyMappingDictionary =
					_propertyMappingService.GetPropertyMapping<InvoiceModel, Invoice>();

				collection = collection.ApplySort(invoiceParameters.OrderBy, exercisePropertyMappingDictionary);
			}


			return await PagedList<Invoice>.Create(
				collection,
				invoiceParameters.PageNumber,
				invoiceParameters.PageSize);
		}

		public async Task<Invoice> GetInvoiceAsync(int invoiceId)
		{
			return await _context.Invoice
			   .Where(l => l.Id == invoiceId)
			   .FirstOrDefaultAsync();
		}

		public async Task<bool> SaveAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public void UpdateInvoice(Invoice invoice)
		{
			//for this purpose context tracking is used
		}
	}
}
