using Microsoft.EntityFrameworkCore;
using Billing.Data.Entities;


namespace Billing.Data;

public class BillContext : DbContext
{

	public BillContext(DbContextOptions<BillContext> options) : base(options)
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
