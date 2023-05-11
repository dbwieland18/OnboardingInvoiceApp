using System.Collections.Generic;
using System.Threading.Tasks;

using AspNetCorePostgreSQLDockerApp.Models;

namespace AspNetCorePostgreSQLDockerApp.Repository
{
    public interface IInvoicesRepository
    {     
        Task<List<Invoice>> GetInvoicesAsync();

        Task<Invoice> GetInvoiceAsync(int id);
        
        Task<Invoice> InsertInvoiceAsync(Invoice invoice);
        Task<bool> UpdateInvoiceAsync(Invoice invoice);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}