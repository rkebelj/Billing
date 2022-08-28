using Billing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Data
{
	public static class BillContextExtensions
	{
        public static void EnsureSeedDataForContext(this BillContext context)
        {
            context.Bill.RemoveRange(context.Bill);

            var workTypes = new List<Bill>()
            {
                new Bill
                {
                    TestCustomer = "nekdo",
                    Progress = 1
                },
                new Bill
                {
                    TestCustomer = "še en",
                    Progress = 33
                }
            };

          

            context.Bill.AddRange(workTypes);

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
