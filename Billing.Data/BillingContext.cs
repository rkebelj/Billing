using Microsoft.EntityFrameworkCore;
using Billing.Data.Entities;


namespace Billing.Data;

public class BillingContext : DbContext
{

	public BillingContext(DbContextOptions<BillingContext> options) : base(options)
	{
		//Database.Migrate();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer("Data Source=WINDOWS-9NI43RU;Initial Catalog=DigitalPMO;Integrated Security=True");
		}
	}



	public DbSet<Bill> Bill { get; set; }

}
