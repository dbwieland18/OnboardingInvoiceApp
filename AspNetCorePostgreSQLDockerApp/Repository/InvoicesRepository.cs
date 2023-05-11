using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using AspNetCorePostgreSQLDockerApp.Models;

namespace AspNetCorePostgreSQLDockerApp.Repository
{
    public class InvoicesRepository : IInvoicesRepository
    {

        private readonly InvoicesDbContext _context;
        private readonly ILogger _logger;

        public InvoicesRepository(InvoicesDbContext context, ILoggerFactory loggerFactory) {
          _context = context;
          _logger = loggerFactory.CreateLogger("InvoicesRepository");
        }

        public async Task<List<Invoice>> GetInvoicesAsync()
        {
            return await _context.Invoices.OrderBy(i => i.Total).ToListAsync();
        }

        public async Task<Invoice> GetInvoiceAsync(int id)
        {
            return await _context.Invoices.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice> InsertInvoiceAsync(Invoice invoice)
        {
            _context.Add(invoice);
            try
            {
              await _context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
               _logger.LogError($"Error in {nameof(InsertInvoiceAsync)}: " + exp.Message);
            }

            return invoice;
        }

        public async Task<bool> UpdateInvoiceAsync(Invoice invoice)
        {
            //Will update all properties of the Invoice
            _context.Invoices.Attach(invoice);
            try
            {
              return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
               _logger.LogError($"Error in {nameof(UpdateInvoiceAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            //Extra hop to the database but keeps it nice and simple for this demo
            var invoice = await _context.Invoices.SingleOrDefaultAsync(i => i.Id == id);
            _context.Remove(invoice);
            try
            {
              return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
               _logger.LogError($"Error in {nameof(DeleteInvoiceAsync)}: " + exp.Message);
            }
            return false;
        }

    }
}