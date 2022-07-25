using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Helpers.ResourceParameters
{
	public class ResourceParametersBase
	{
		protected virtual int maxPageSize { get { return 10; } }
		public int PageNumber { get; set; } = 1;
		protected virtual int _pageSize { get; set; } = 5;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
		public string? SearchQuery { get; set; }
		public string? Fields { get; set; }
		public string? OrderBy { get; set; }
	}
}
