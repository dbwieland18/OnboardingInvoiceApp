using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCorePostgreSQLDockerApp.Models;
using AspNetCorePostgreSQLDockerApp.Repository;
using Microsoft.AspNetCore.Http;

namespace AspNetCorePostgreSQLDockerApp.Apis
{
    [Route("api/[controller]/invoices")]
    public class InvoicesServiceController : Controller
    {
        IInvoicesRepository _repo;

        public InvoicesServiceController(IInvoicesRepository repo) {
          _repo = repo;
        }

        // GET api/dataservice/invoices
        [HttpGet()]
        [ProducesResponseType(typeof(List<Invoice>), 200)]
        [ProducesResponseType(typeof(List<Invoice>), 404)]
        public async Task<ActionResult> Invoices()
        {
            AspNetCorePostgreSQLDockerApp.Producer.Producer.Start();
            
            var invoices = await _repo.GetInvoicesAsync();
            if (invoices == null) {
              return NotFound();
            }
            return Ok(invoices);
        }

        // GET api/dataservice/invoices/5
        [HttpGet("{id}", Name = "GetInvoicesRoute")]
        [ProducesResponseType(typeof(Invoice), 200)]
        [ProducesResponseType(typeof(Invoice), 404)]
        public async Task<ActionResult> Invoices(int id)
        {
            var invoice = await _repo.GetInvoiceAsync(id);
            if (invoice == null) {
              return NotFound();
            }
            return Ok(invoice);
        }

        // POST api/invoices
        [HttpPost()]
        [ProducesResponseType(typeof(Invoice), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> PostInvoice([FromBody]Invoice invoice)
        {
          if (!ModelState.IsValid) {
            return BadRequest(this.ModelState);
          }

          var newInvoice = await _repo.InsertInvoiceAsync(invoice);
          if (newInvoice == null) {
            return BadRequest("Unable to insert invoice");
          }
          return CreatedAtRoute("GetInvoicesRoute", new { id = newInvoice.Id}, newInvoice);
        }

        // PUT api/dataservice/invoices/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 400)]
        public async Task<ActionResult> PutInvoice(int id, [FromBody]Invoice invoice)
        {
          if (!ModelState.IsValid) {
            return BadRequest(this.ModelState);
          }

          var status = await _repo.UpdateInvoiceAsync(invoice);
          if (!status) {
            return BadRequest("Unable to update invoice");
          }
          return Ok(status);
        }

        // DELETE api/dataservice/invoices/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 404)]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
          var status = await _repo.DeleteInvoiceAsync(id);
          if (!status) {
            return NotFound();
          }
          return Ok(status);
        }

    }

    public static class HttpRequestExtensions
    {
        public static Uri ToUri(this HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }
    }
}
