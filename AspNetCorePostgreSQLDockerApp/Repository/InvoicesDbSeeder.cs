using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AspNetCorePostgreSQLDockerApp.Models;

namespace AspNetCorePostgreSQLDockerApp.Repository
{
    public class InvoicesDbSeeder
    {
        readonly ILogger _logger;

        public InvoicesDbSeeder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("InvoicesDbSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            //Based on EF team's example at https://github.com/aspnet/MusicStore/blob/dev/samples/MusicStore/Models/SampleData.cs
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var invoiceDb = serviceScope.ServiceProvider.GetService<InvoicesDbContext>();
                if (await invoiceDb.Database.EnsureCreatedAsync())
                {
                    if (!await invoiceDb.Invoices.AnyAsync()) {
                      await InsertInvoiceSampleData(invoiceDb);
                    }
                }
            }
        }

        public async Task InsertInvoiceSampleData(InvoicesDbContext db)
        {
            var invoices = GetInvoices();
            db.Invoices.AddRange(invoices);

            try
            {
              await db.SaveChangesAsync();
            }
            catch (Exception exp)
            {
              _logger.LogError($"Error in {nameof(InvoicesDbSeeder)}: " + exp.Message);
              throw;
            }

        }

        private List<Invoice> GetInvoices() {
            //Invoices
            var invoices = new List<Invoice> 
            {
                new Invoice { Total = 998.79, Paid = false },
                new Invoice { Total = 6.02, Paid = false },
                new Invoice { Total = 328.29, Paid = true }
            };

            return invoices;
        }
    }
}