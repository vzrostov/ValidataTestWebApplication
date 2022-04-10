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

namespace ValidataTestWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private InternalUnitOfWork unitOfWork = new InternalUnitOfWork();

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var orders = unitOfWork.OrderRepository.GetAll(); 
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await unitOfWork.OrderRepository.GetAsync((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(unitOfWork.DbContext.Customers, "CustomerID", "FirstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderId,Date,Price,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.OrderRepository.Create(order);
                await unitOfWork.SaveChangesAsync(); // TODO сделать везде обработку исключения, проверить возвращаемое значение
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(unitOfWork.DbContext.Customers, "CustomerID", "FirstName", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await unitOfWork.OrderRepository.GetAsync((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(unitOfWork.DbContext.Customers, "CustomerID", "FirstName", order.CustomerID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderId,Date,Price,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.OrderRepository.Update(order);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(unitOfWork.DbContext.Customers, "CustomerID", "FirstName", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await unitOfWork.OrderRepository.GetAsync((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await unitOfWork.OrderRepository.GetAsync(id);
            unitOfWork.OrderRepository.Delete(order);
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
