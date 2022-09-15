using Microsoft.EntityFrameworkCore;
using Billing.Data.Entities;
using Billing.Models.InvoiceMaterial;
using Billing.Models.Meeting;
using Billing.Models.Invoice;
using Billing.Models.Material;

namespace Billing.Data;

public class BillingContext : DbContext
{

	public BillingContext(DbContextOptions<BillingContext> options) : base(options)
	{
		//Database.Migrate();
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<InvoiceModel>()
			.HasMany(i => i.Materials)
			.WithMany(m => m.Invoices)
			.UsingEntity<InvoiceMaterialModel>
			(im => im.HasOne<MaterialModel>().WithMany(),
			 im => im.HasOne<InvoiceModel>().WithMany())
			.Property(im => im.Quantity)
			.HasDefaultValue(1);
			

		//modelBuilder.Entity<InvoiceMaterialModel>()
		//	.HasOne(i => i.Invoice)
		//	.WithMany(im => im.InvoiceMaterials)
		//	.HasForeignKey(b => b.InvoiceId);



	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer("Data Source=WINDOWS-9NI43RU;Initial Catalog=DigitalPMO;Integrated Security=True");
		}
	}



	public DbSet<Invoice> Invoice { get; set; }
	public DbSet<Material> Material { get; set; }
	public DbSet<MeetingModel> Meeting { get; set; }

}
