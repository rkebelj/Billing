using Microsoft.EntityFrameworkCore;
using Billing.Data.Entities;


namespace Billing.Data;

public class InvoiceContext : DbContext
{

	public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options)
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



	public DbSet<Invoice> Invoice { get; set; }

}
