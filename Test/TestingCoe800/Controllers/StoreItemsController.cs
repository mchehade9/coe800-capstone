using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestingCoe800.Models;

namespace TestingCoe800.Controllers
{
    [Authorize(Roles = "Manager")]
    public class StoreItemsController : Controller
    {
      
        private UsersDBEntities db = new UsersDBEntities();

        
        // GET: Store__Items
        public ActionResult Index()
        {
            string Manager_Id = User.Identity.GetUserId().ToString();

            var storeItems = db.StoreItems.Include(s => s.Store).Where(s=> s.Store.ManagerIDFk == Manager_Id);

            return View(storeItems.ToList());
        }

        // GET: Store__Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // GET: Store__Items/Create
        public ActionResult Create()
        {
         string Manager_Id = User.Identity.GetUserId().ToString();
        ViewBag.StoreIDFk = new SelectList(db.Stores.Where(g => g.ManagerIDFk.ToString() == Manager_Id ) , "Id", "StoreName");
            return View();
        }

        //import .csv file for store directory dump
        public ActionResult Import() 
        {
            //ViewBag.StoreIDFK = new 
            return View();
        }

        // POST: Store__Items/Create      
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemName,StoreIDFk,Price")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                db.StoreItems.Add(storeItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.StoreIDFk = new SelectList(db.Stores, "Id", "StoreName", storeItem.StoreIDFk);
            return View(storeItem);
        }

        // GET: Store__Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            string Manager_Id = User.Identity.GetUserId().ToString();
            ViewBag.StoreIDFk = new SelectList(db.Stores.Where(g => g.ManagerIDFk.ToString() == Manager_Id), storeItem.StoreIDFk);
            return View(storeItem);
        }

        // POST: Store__Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,Price")] StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                storeItem.StoreIDFk = db.StoreItems.Where(d => d.Id  == storeItem.Id).Select(d => d.StoreIDFk).SingleOrDefault();
                db.Entry(storeItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreIDFk = new SelectList(db.Stores, "Id", "StoreName", storeItem.StoreIDFk);
            return View(storeItem);
        }

        // GET: Store__Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = db.StoreItems.Find(id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: Store__Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreItem storeItem = db.StoreItems.Find(id);
            db.StoreItems.Remove(storeItem);
            db.SaveChanges();
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
