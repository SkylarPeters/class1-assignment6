using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237_assignment6.Models;

namespace cis237_assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageContext db = new BeverageContext();

        // GET: Beverages
        public ActionResult Index()
        {
            // Setup a variable to gold the beverages data.
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            // Setup some strings to hold the data that might be
            // in the session. If there is nothing in the session
            // we can still use these variables as a default value.
            string filterName = "";
            string filterMin = "";
            string filterMax = "";
            // Define a min and max for the price
            decimal min = 0;
            decimal max = 1000;

            // Check to see if there is a value in the session,
            // and if there is, assign it to the variable that
            // we set up to hold the value.
            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_name"]
            ))
            {
                filterName = (string)Session["session_name"];
            }

            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_min"]
            ))
            {
                filterMin = (string)Session["session_min"];
                min = decimal.Parse(filterMin);
            }

            if (!string.IsNullOrWhiteSpace(
            (string)Session["session_max"]
            ))
            {
                filterMax = (string)Session["session_max"];
                max = decimal.Parse(filterMax);

            }

            // Do the filter on the BeveragesToFilter Dataset.
            // Use the where that we used before doing
            // the last inclass, only this tume send in more
            // lambda expressions to narrow it down further.
            // Since we set up the default values for each of the
            // filter parameters, min, max, and filterName, we
            // can count on this always running with no errors.
            IList<Beverage> finalFiltered = BeveragesToFilter.Where(
                Beverage => Beverage.price >= min &&
                Beverage.price <= max &&
                Beverage.name.Contains(filterName)
            ).ToList();

            // Place the string representation of the values
            // that are in the session into the viewbag so
            // that they can be retrieved and displayed on the view.
            ViewBag.filterName = filterName;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;


            // Return the view with the filtered selection of cars.
            return View(finalFiltered);

            //return View(db.Beverages.ToList());
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Mark the method as Post since it is reached from a form submit
        // Make sure to validate the antiforgery token.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            // Get the form data that we sent out of the request object.
            // The string that is used as a key to get the data matches
            // the name property of the form control
            string name = Request.Form.Get("name");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            // Now that we have the data pulled out from the 
            // request object, let's put it into the session
            // so that other methods can have access to it.
            Session["session_name"] = name;
            Session["session_min"] = min;
            Session["session_max"] = max;


            // Redirect to the index page
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
