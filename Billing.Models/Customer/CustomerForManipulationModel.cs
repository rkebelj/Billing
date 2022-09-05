using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Models.Customer
{
	public abstract class CustomerForManipulationModel
	{
		[Required]
		[MaxLength(255, ErrorMessage = "Firstname shouldn't have more than 255 characters.")]

		public int FirstName { get; set; }
		[Required]
		[MaxLength(255, ErrorMessage = "Lastname shouldn't have more than 255 characters.")]

		public string? LastName { get; set; }
		[Required]
		[MaxLength(255, ErrorMessage = "Address shouldn't have more than 255 characters.")]
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? PostalCode { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
	}
}
