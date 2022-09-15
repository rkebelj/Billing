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

builder.Services.AddDbContext<BillingContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();
builder.Services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

builder.Services.AddTransient<IInvoiceRepo, InvoiceRepo>();
builder.Services.AddTransient<IMaterialRepo, MaterialRepo>();
//builder.Services.AddTransient<ICustomFinanceDataRepo, CustomFinanceDataRepo>();
//builder.Services.AddTransient<IWorkTypeRepo, WorkTypeRepo>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(implementationFactory =>
{
	var actionContext =
	implementationFactory.GetRequiredService<IActionContextAccessor>().ActionContext;
	var factory = implementationFactory.GetRequiredService<IUrlHelperFactory>();
	return factory.GetUrlHelper(actionContext);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();





var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<BillingContext>();
	dbContext.EnsureSeedDataForContext();
	// use context
}


app.Run();

