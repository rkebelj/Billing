using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Helpers.ResourceParameters
{
	public class MaterialResourceParameters : ResourceParametersBase
	{
		protected override int maxPageSize { get { return 1000; } }

		protected override int _pageSize { get; set; } = 100;
	}
}
