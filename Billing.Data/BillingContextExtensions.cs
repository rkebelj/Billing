using Billing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data
{
	public static class BillingContextExtensions
	{
        public static void EnsureSeedDataForContext(this InvoiceContext context)
        {
            context.Invoice.RemoveRange(context.Invoice);

            var workTypes = new List<Invoice>()
            {
                new Invoice
                {
                    TestCustomer = "nekdo",
                    Progress = "454"
                },
                new Invoice
                {
                    TestCustomer = "še en",
                    Progress = "77"
                }
            };

          

            context.Invoice.AddRange(workTypes);

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
