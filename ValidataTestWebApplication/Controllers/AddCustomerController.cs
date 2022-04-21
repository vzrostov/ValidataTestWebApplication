using System.Threading.Tasks;
using System.Web.Mvc;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.Controllers
{
    public class AddCustomerController : Controller
    {
        /// <summary>
        /// For deleting demonstration. Not the best decision
        /// </summary>
        internal static int LastAddedID = 0;

        // GET: AddCustomer
        public async Task<ActionResult> Index()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var customer = new Customer("test Name", "test LastName", "test Address", "test Code", null);
            var result = await unitOfWork.CreateCustomerAsync(customer);
            LastAddedID = customer.CustomerID;
            return View(new AsyncDbTaskResultForCreate() { NewId = customer.CustomerID, TaskResult = result });
        }
    }
}