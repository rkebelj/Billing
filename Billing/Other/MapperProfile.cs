using AutoMapper;
using Billing.Data.Entities;
using Billing.Models;

namespace Billing.API.Other

{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Bill, BillModel>();
		
		}
	}
}
