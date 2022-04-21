using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.Controllers
{
    public class DeleteCustomerController : Controller
    {
        // GET: DeleteCustomer
        public async Task<ActionResult> Index()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var customer = new Customer("test Name", "test LastName", "test Address", "test Code", null) 
                { CustomerID = AddCustomerController.LastAddedID };
            var result = await unitOfWork.DeleteCustomerAsync(customer);
            return View(new AsyncDbTaskResult() { TaskResult = result });
        }
    }
}