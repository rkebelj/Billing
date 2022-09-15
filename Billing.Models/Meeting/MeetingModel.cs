using Billing.Models.InvoiceMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Models.Meeting
{
	public class MeetingModel
	{
			public int Id { get; set; }
			public int CustomerId { get; set; }//FK
			public DateTime MeetingDate{ get; set; }
			public DateTime? ExpectedDeadline { get; set; } = DateTime.UtcNow.AddMonths(2);
			public string? Unit { get; set; }
			public string? Comment { get; set; }

			public IList<InvoiceMaterialModel>? InvoiceMaterials { get; set; }

	}
}
