using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCorePostgreSQLDockerApp.Controllers
{
    public class InvoicesController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
