using AutoMapper;
using Billing.Data.Entities;
using Billing.Models;
using Billing.Models.Invoice;
using Billing.Models.Material;

namespace Billing.API.Other

{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			//invoice
			CreateMap<Invoice, InvoiceModel>();
			CreateMap<Invoice, InvoiceForUpdateModel>();
			CreateMap<InvoiceForCreationModel , Invoice>();
			CreateMap<InvoiceForUpdateModel, Invoice>();

			//material
			CreateMap<Material, MaterialModel>();

		}
	}
}
