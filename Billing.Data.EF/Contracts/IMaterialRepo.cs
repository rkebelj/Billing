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
	public  interface IMaterialRepo
	{
		Task<PagedList<Material>> GetMaterialAsync(MaterialResourceParameters MaterialParameters);
		Task<Material> GetMaterialAsync(int MaterialId);
		Task AddMaterialAsync(Material Material);
		void UpdateMaterial(Material Material);
		Task<bool> MaterialExistsAsync(int MaterialId);
		void DeleteMaterial(Material Material);
		Task<bool> SaveAsync();

	}
}
