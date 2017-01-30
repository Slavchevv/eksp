using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eksp.Models;
using Microsoft.AspNet.Identity;

namespace eksp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index()
        {

            //
            string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();
            db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //
            var usersDetails = db.UsersDetails.Include(u => u.Company);
            return View(usersDetails);
        }


        public ActionResult UserProfile()
        {

            //
            string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();
            db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //
           // var usersDetails = db.UsersDetails.Include(u => u.Company);
            return View(userDetails);
        }
        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UsersDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserDetailsId,ImageData,FirstName,LastName,UserAddress,UserCountry,UserPostalCode,UserPhoneNumber,CompanyId,identtyUserId")] UserDetails userDetails, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] buf = new byte[UploadImage.ContentLength];
                UploadImage.InputStream.Read(buf, 0, buf.Length);
                userDetails.ImageData = buf;

                db.UsersDetails.Add(userDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetails);
        }

        //public ActionResult Create([Bind(Include = "UserDetailsId,ImageData,FirstName,LastName,UserAddress,UserCountry,UserPostalCode,UserPhoneNumber,CompanyId,identtyUserId")] UserDetails userDetails)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UsersDetails.Add(userDetails);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", userDetails.CompanyId);
        //    return View(userDetails);
        //}

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UsersDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", userDetails.CompanyId);
            return View(userDetails);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserDetailsId,ImageData,FirstName,LastName,UserAddress,UserCountry,UserPostalCode,UserPhoneNumber,CompanyId,identtyUserId")] UserDetails userDetails, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] buf = new byte[UploadImage.ContentLength];
                UploadImage.InputStream.Read(buf, 0, buf.Length);
                userDetails.ImageData = buf;

                db.Entry(userDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
               
            }
            //ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", userDetails.CompanyId);
            return View(userDetails);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UsersDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDetails userDetails = db.UsersDetails.Find(id);
            db.UsersDetails.Remove(userDetails);
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
