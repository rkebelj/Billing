using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Billing.Models.Bill
{
	public abstract class BillForManipulationModel
	{

        [MaxLength(100, ErrorMessage = "Test Customer shouldn't have more than 100 characters.")]
        public string? TestCustomer { get; set; }
        [MaxLength(255, ErrorMessage = "Progress  shouldn't have more than 255 characters.")]
        public string? Progress { get; set; }
    }
}
