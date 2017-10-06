using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPMVCCallingStoredProceduresInEF6.Models;
using System.Data.SqlClient;

namespace ASPMVCCallingStoredProceduresInEF6.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerEntities db = new CustomerEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Database.SqlQuery<Customer>("GetAllCustomers").ToList());
        }
        
        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyName,ContactName,Address,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("EXEC dbo.InsertCustomer @CompanyName,@ContactName,@Address,@Country,@Phone",
                    new SqlParameter("CompanyName",customer.CompanyName),
                    new SqlParameter("ContactName", customer.ContactName),
                    new SqlParameter("Address", customer.Address),
                    new SqlParameter("Country", customer.Country),
                    new SqlParameter("Phone", customer.Phone));

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CompanyName,ContactName,Address,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("EXEC dbo.UpdateCustomer @CustomerID,@CompanyName,@ContactName,@Address,@Country,@Phone",
                  new SqlParameter("CustomerID", customer.CustomerID),
                  new SqlParameter("CompanyName", customer.CompanyName),
                  new SqlParameter("ContactName", customer.ContactName),
                  new SqlParameter("Address", customer.Address),
                  new SqlParameter("Country", customer.Country),
                  new SqlParameter("Phone", customer.Phone));

                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Database.ExecuteSqlCommand("EXEC dbo.DeleteCustomer @CustomerID",
                    new SqlParameter("CustomerID", id));

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
