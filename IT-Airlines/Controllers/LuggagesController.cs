﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IT_Airlines.DataContexts;
using IT_Airlines.Models.Entities;

namespace IT_Airlines.Controllers
{
    public class LuggagesController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Luggages
        public ActionResult Index()
        {
            return View(db.Luggages.ToList());
        }

        // GET: Luggages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luggage luggage = db.Luggages.Find(id);
            if (luggage == null)
            {
                return HttpNotFound();
            }
            return View(luggage);
        }

        // GET: Luggages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Luggages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaximumWeight,Price")] Luggage luggage)
        {
            if (ModelState.IsValid)
            {
                db.Luggages.Add(luggage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(luggage);
        }

        // GET: Luggages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luggage luggage = db.Luggages.Find(id);
            if (luggage == null)
            {
                return HttpNotFound();
            }
            return View(luggage);
        }

        // POST: Luggages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaximumWeight,Price")] Luggage luggage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(luggage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(luggage);
        }

        // GET: Luggages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luggage luggage = db.Luggages.Find(id);
            if (luggage == null)
            {
                return HttpNotFound();
            }
            return View(luggage);
        }

        // POST: Luggages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Luggage luggage = db.Luggages.Find(id);
            db.Luggages.Remove(luggage);
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