using AccountingWebsite.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace AccountingWebsite
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<TransactionDataContext>(options => options.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("TransactionsConnection")));
			builder.Services.AddDbContext<VendorContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("VendorConnection")));
			builder.Services.AddDbContext<StatementContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("StatementConnection")));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Remove Limit on Forms
            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if(!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// Create the database if it does not exist
			using(var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				var transContext = services.GetRequiredService<TransactionDataContext>();
				transContext.Database.EnsureCreated();
				var vendorContext = services.GetRequiredService<VendorContext>();
                vendorContext.Database.EnsureCreated();
				DbInitializer.Initialize(transContext, vendorContext);
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "receipt_images")),
                RequestPath = new PathString("/receipt_images")
            });

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}