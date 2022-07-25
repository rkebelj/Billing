using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Models.Bill
{
	public class BillForCreationModel: BillForManipulationModel
	{
		[Required(ErrorMessage = "Project ID should not be empty.")]
		public int BillId { get; set; }
	}
}
