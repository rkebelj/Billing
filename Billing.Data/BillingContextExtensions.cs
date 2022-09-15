using Billing.Data.Entities;

namespace Billing.Data
{
	public static class BillingContextExtensions
	{
		public static void EnsureSeedDataForContext(this BillingContext context)
		{
			context.Invoice.RemoveRange(context.Invoice);
			context.Material.RemoveRange(context.Material);
			context.SaveChanges();

			var invoices = new List<Invoice>()
			{
				new Invoice
				{
				   CustomerId = 1,
				   Discount = 1,
				   Date = DateTime.Now,
				},
				new Invoice
				{
				   CustomerId = 1,
				   Discount = 1,
				   Date = DateTime.Today.AddDays(13),
				},
				new Invoice
				{
				   CustomerId = 1,
				   Discount = 50,
				   Date = DateTime.Today.AddDays(5),
				}
			};
			var materials = new List<Material>()
			{
				new Material
				{
				 CategoryId = 1,
				 Comment = "komentar",
					 Name = "Dilca",
				 Price = 10,
				 Unit = "komad",
				},
				new Material
				{
				 CategoryId = 2,
				 Comment = "Tulele je kr uredu",
				 Name = "Vogal",
				 Price = 35,
				 Unit = "komad",
				},
				new Material
				{
				 CategoryId = 2,
				 Comment = "Vredi fredi?",
				 Name = "Regal",
				 Price = 4,
				 Unit = "komad",
				}

			};



			context.Invoice.AddRange(invoices);
			context.Material.AddRange(materials);

			//using (var transaction = context.Database.BeginTransaction())
			//{
			//    context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Authors] ON");
			//    context.SaveChanges();
			//    context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Authors] OFF");

			//    transaction.Commit();
			//}
			context.SaveChanges();
		}
	}
}
