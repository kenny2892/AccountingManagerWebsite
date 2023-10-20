using AccountingWebsite.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			builder.Services.AddDbContext<TransactionDataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

				var context = services.GetRequiredService<TransactionDataContext>();
				context.Database.EnsureCreated();
				DbInitializer.Initialize(context);
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}