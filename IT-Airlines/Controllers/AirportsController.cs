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
using IT_Airlines.Models.UserRoles;
using PagedList;
using PagedList.Mvc;

namespace IT_Airlines.Controllers
{
    public class AirportsController : Controller
    {
        private AirlineDbContext db = new AirlineDbContext();

        // GET: Airports
        public ActionResult Index(string searchBy, string search, int? page, string sortBy = "Code desc")
        {
            ViewBag.SortCodeParameter = sortBy == "Code asc" ? "Code desc" : "Code asc";
            ViewBag.SortNameParameter = sortBy == "Name asc" ? "Name desc" : "Name asc";
            ViewBag.SortCityParameter = sortBy == "City asc" ? "City desc" : "City asc";

            var airports = db.Airports.AsQueryable();

            if(searchBy == "Name" && !string.IsNullOrEmpty(search))
            {
                airports = airports.Where(n => n.Name.ToLower().StartsWith(search.ToLower()));
            }
            else if (searchBy == "City" && !string.IsNullOrEmpty(search))
            {
                airports = airports.Where(n => n.City.ToLower().StartsWith(search.ToLower()));
            }

            switch (sortBy)
            {
                case "Code desc":
                    airports = airports.OrderByDescending(d => d.Code);
                    break;
                case "Code asc":
                    airports = airports.OrderBy(d => d.Code);
                    break;
                case "Name desc":
                    airports = airports.OrderByDescending(d => d.Name);
                    break;
                case "Name asc":
                    airports = airports.OrderBy(d => d.Name);
                    break;
                case "City desc":
                    airports = airports.OrderByDescending(d => d.City);
                    break;
                default:
                    airports = airports.OrderBy(d => d.City);
                    break;
            }

            IPagedList<Airport> airports1 = airports.ToPagedList(page ?? 1, 5);
            return View(airports1);
        }

        // GET: Airports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        // GET: Airports/Create
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Create([Bind(Include = "Id,Code,Name,City")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                db.Airports.Add(airport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airport);
        }

        // GET: Airports/Edit/5
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        // POST: Airports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,City")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airport);
        }

        // GET: Airports/Delete/5
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.Find(id);
            if (airport == null)
            {
                return HttpNotFound();
            }
            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = Roles.Administrator + ", " + Roles.Moderator)]
        public ActionResult DeleteConfirmed(int id)
        {
            Airport airport = db.Airports.Find(id);

            var flights = db.Flights.Where(f => f.AirportFrom.Id == airport.Id || f.AirportTo.Id == airport.Id).ToList();

            var flightsId = flights.Select(f => f.Id).ToList();
            var reservations = new List<Reservation>();
            reservations = db.Reservations.Where(r => flightsId.Contains(r.FirstFlight.Id) || flightsId.Contains(r.SecondFlight.Id)).ToList();

            for (int i = 0; i < reservations.Count; i++)
            {
                db.Reservations.Remove(reservations[i]);
                db.SaveChanges();
            }

            for (int i = 0; i < flights.Count; i++)
            {
                db.Flights.Remove(flights[i]);
                db.SaveChanges();
            }

            db.Airports.Remove(airport);
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
