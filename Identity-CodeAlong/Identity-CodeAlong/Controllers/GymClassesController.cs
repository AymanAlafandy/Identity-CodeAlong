﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Identity_CodeAlong.Models;

namespace Identity_CodeAlong.Controllers
{
    //[Authorize]
    public class GymClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //[AllowAnonymous]                                                                     //added allow anonymous to see the list for visitors
        // GET: GymClasses
        public ActionResult Index()
        {
            List<GymClassIndexModel> model = new List<GymClassIndexModel>();
            foreach (GymClass gc in db.GymClasses.Where(x => x.StartTime >= DateTime.Now).ToList())
            {
                GymClassIndexModel index = new GymClassIndexModel();
                index.Id = gc.Id;
                index.Name = gc.Name;
                index.StartTime = gc.StartTime;
                index.Duration = gc.Duration;
                index.Description = gc.Description;
                if (gc.AttendingMembers.FirstOrDefault(x =>
                 x.UserName == User.Identity.Name) == null)
                {
                    index.Attending = "Attend";
                }
                else
                {
                    index.Attending = "Cancel";
                }
                model.Add(index);
            }
            return View(model);
        }

        [Authorize]
        public ActionResult BookedClasses()
        {
            List<GymClassIndexModel> model = new List<GymClassIndexModel>();
            foreach (GymClass gc in db.GymClasses.Where(x =>
            x.StartTime >= DateTime.Now &&
            x.AttendingMembers.FirstOrDefault(c =>
            c.UserName == User.Identity.Name) != null).ToList())
            {
                GymClassIndexModel index = new GymClassIndexModel();
                index.Id = gc.Id;
                index.Name = gc.Name;
                index.StartTime = gc.StartTime;
                index.Duration = gc.Duration;
                index.Description = gc.Description;
                index.Attending = "Cancel";

                model.Add(index);
            }
            return View(model);
        }

        [Authorize]
        public ActionResult PersonalHistory()
        {
            List<GymClassIndexModel> model = new List<GymClassIndexModel>();
            foreach (GymClass gc in db.GymClasses.Where(x =>
            x.StartTime < DateTime.Now &&
            x.AttendingMembers.FirstOrDefault(c =>
            c.UserName == User.Identity.Name) != null).ToList())
            {
                GymClassIndexModel index = new GymClassIndexModel();
                index.Id = gc.Id;
                index.Name = gc.Name;
                index.StartTime = gc.StartTime;
                index.Duration = gc.Duration;
                index.Description = gc.Description;

                model.Add(index);
            }
            return View(model);
        }


        // GET: GymClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }
         
        [Authorize(Roles = "Admin")]
                                                                                                                                //Added this to block access for visitors---(Roles ="Admin")

        // GET: GymClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.GymClasses.Add(gymClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        // GET: GymClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }


        [Authorize(Roles = "Admin")]
        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]

        // GET: GymClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }


        [Authorize(Roles = "Admin")]
        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GymClass gymClass = db.GymClasses.Find(id);
            db.GymClasses.Remove(gymClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult BookingToggle(int id)
        {

            //GymClass CurrentClass = db.GymClasses.Where(x => x.Id == id).FirstOrDefault();                                                                        //we should not write like dis,firstor default matches the data or it will return null otherwise the program will crashso we ned to give as below
            //ApplicationUser CurrentUser = db.Users.Where(g => g.UserName == User.Identity.Name).FirstOrDefault();

            GymClass CurrentClass = db.GymClasses.FirstOrDefault(x => x.Id == id);
            ApplicationUser CurrentUser = db.Users.FirstOrDefault(x => x.UserName== User.Identity.Name);                    //firstordefault will always return something,but if we use first wont do like dis 


            if (CurrentClass.AttendingMembers.Contains(CurrentUser))
            {
                CurrentClass.AttendingMembers.Remove(CurrentUser);
                db.SaveChanges();
            }
            else
            {
                CurrentClass.AttendingMembers.Add(CurrentUser);
                db.SaveChanges();
            }
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
