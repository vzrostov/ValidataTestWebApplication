using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ValidataTest.Core.DAL;
using ValidataTest.Core.Models;

namespace ValidataTestWebApplication.Controllers
{
    public class ItemsController : Controller
    {
        private UnitOfWork unitOfWork;

        public ItemsController()
        {
            unitOfWork = new UnitOfWork();
        }

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var items = unitOfWork.GetItems();
            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.GetItemAsync((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(unitOfWork.GetOrders(), "OrderId", "OrderId");
            ViewBag.ProductID = new SelectList(unitOfWork.GetProducts(), "ProductId", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemId,Quantity,ProductID,OrderID")] Item item)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(unitOfWork.GetOrders(), "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.GetProducts(), "ProductId", "Name", item.ProductID);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.GetItemAsync((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(unitOfWork.GetOrders(), "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.GetProducts(), "ProductId", "Name", item.ProductID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemId,Quantity,ProductID,OrderID")] Item item)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.UpdateItemAsync(item);
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(unitOfWork.GetOrders(), "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.GetProducts(), "ProductId", "Name", item.ProductID);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.GetItemAsync((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await unitOfWork.GetItemAsync(id);
            await unitOfWork.DeleteItemAsync(item);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
