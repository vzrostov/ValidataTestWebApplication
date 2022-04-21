using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.Controllers
{
    public class AddOrderController : Controller
    {
        // GET: AddOrder
        public async Task<ActionResult> Index()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var order = new Order(DateTime.Now, AddCustomerController.LastAddedID, null,
                    new ReadOnlyCollection<Item>(
                        new List<Item>()
                        {
                            new Item(10f, 1, null, 0, null),
                            new Item(01f, 2, null, 0, null)
                        }));
            var result = await unitOfWork.CreateOrderAsync(order);
            return View(new AsyncDbTaskResultForCreate() { NewId = order.OrderId, TaskResult = result });
        }
    }
}