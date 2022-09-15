using Billing.Data.EF.Contracts;
using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data.EF.Implementations
{
	public class MaterialRepo : IMaterialRepo
	{
		public Task AddMaterialAsync(Material Material)
		{
			throw new NotImplementedException();
		}

		public void DeleteMaterial(Material Material)
		{
			throw new NotImplementedException();
		}

		public Task<PagedList<Material>> GetMaterialAsync(MaterialResourceParameters MaterialParameters)
		{
			throw new NotImplementedException();
		}

		public Task<Material> GetMaterialAsync(int MaterialId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> MaterialExistsAsync(int MaterialId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> SaveAsync()
		{
			throw new NotImplementedException();
		}

		public void UpdateMaterial(Material Material)
		{
			throw new NotImplementedException();
		}
	}
}
