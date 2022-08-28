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
			CreateMap<Bill, BillModel>();
			CreateMap<Bill, BillForUpdateModel>();
			CreateMap<BillForCreationModel, Bill>();
			CreateMap<BillForUpdateModel, Bill>();
		
		}
	}
}
