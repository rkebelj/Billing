using AutoMapper;
using Billing.Data.Entities;
using Billing.Models;
using Billing.Models.Bill;

namespace Billing.API.Other

{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Invoice, InvoiceModel>();
			CreateMap<Invoice, InvoiceForUpdateModel>();
			CreateMap<InvoiceForCreationModel , Invoice>();
			CreateMap<InvoiceForUpdateModel, Invoice>();
		
		}
	}
}
