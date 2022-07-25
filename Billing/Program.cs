using Billing.Business.Contracts;
using Billing.Business.Implementations;
using Billing.Data;
using Billing.Data.EF.Contracts;
using Billing.Data.EF.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews();


builder.Services.AddControllers(options =>
{
	options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters().AddControllersAsServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<BillContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();
builder.Services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

builder.Services.AddTransient<IBillRepo, BillRepo>();
//builder.Services.AddTransient<ICustomFinanceDataRepo, CustomFinanceDataRepo>();
//builder.Services.AddTransient<IWorkTypeRepo, WorkTypeRepo>();




var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<BillContext>();
	dbContext.EnsureSeedDataForContext();
	// use context
}


app.Run();

