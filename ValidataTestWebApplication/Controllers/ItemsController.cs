using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ValidataTestWebApplication.DAL;
using ValidataTestWebApplication.Models;
using ValidataTest.Core.Models;

namespace ValidataTestWebApplication.Controllers
{
    public class ItemsController : Controller
    {
        private InternalUnitOfWork unitOfWork;

        public ItemsController()
        {
            unitOfWork = new InternalUnitOfWork();
        }

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var items = unitOfWork.ItemRepository.GetAll();
            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.ItemRepository.GetAsync((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(unitOfWork.DbContext.Orders, "OrderId", "OrderId");
            ViewBag.ProductID = new SelectList(unitOfWork.DbContext.Products, "ProductId", "Name");
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
                unitOfWork.ItemRepository.Create(item);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(unitOfWork.DbContext.Orders, "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.DbContext.Products, "ProductId", "Name", item.ProductID);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.ItemRepository.GetAsync((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(unitOfWork.DbContext.Orders, "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.DbContext.Products, "ProductId", "Name", item.ProductID);
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
                unitOfWork.ItemRepository.Update(item);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(unitOfWork.DbContext.Orders, "OrderId", "OrderId", item.OrderID);
            ViewBag.ProductID = new SelectList(unitOfWork.DbContext.Products, "ProductId", "Name", item.ProductID);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await unitOfWork.ItemRepository.GetAsync((int)id);
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
            Item item = await unitOfWork.ItemRepository.GetAsync(id);
            unitOfWork.ItemRepository.Delete(item);
            await unitOfWork.SaveChangesAsync();
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
