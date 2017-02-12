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
using Newtonsoft.Json;

namespace eksp.Controllers
{
    [Authorize(Roles = "Admin, CompanyAdministrator")]
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
           
            return View();
        }

        // GET: Companies/Details/5
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult Company()
        {
            //if not in role siteAdmin
            //return company where CAID == identuty.user.id
            string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            Company company = db.Companies.Where(c => c.CAId == currentUserId)
                    .FirstOrDefault();
            //db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //return View(db.Companies.ToList());
            //Company company = db.Companies.Where(c => c.CAId == currentUserId
                    //&& c.ImageBase64 != null
                    //&& c.ImageData?.Length > 0).FirstOrDefault();

            return View(company);
        }


        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Company company = db.Companies.Find(id);
        //    if (company == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(company);
        //}

        // GET: Companies/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyId,ImageData,CompanyName,CompanyAddress,CompanyCountry,CompanyCity,CompanyPostalCode,CompanyPhoneNumber,CAId")] Company company)//
        {
            if (ModelState.IsValid)
            {
                //byte[] buf = new byte[UploadImage.ContentLength];
                //UploadImage.InputStream.Read(buf, 0, buf.Length);
                //company.ImageData = buf;

                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userid = User.Identity.GetUserId();
            //Company company = db.Companies.Find(id);
            Company company = db.Companies.Where(i => i.CAId == userid).First();
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "CompanyAdministrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,ImageData,CompanyName,CompanyAddress,CompanyCountry,CompanyCity,CompanyPostalCode,CompanyPhoneNumber,CAId")] Company company, HttpPostedFileBase UploadImage)
        {           
            if (ModelState.IsValid)
            {
                var companiesFromDb = db.Companies.Where(u => u.CAId == company.CAId).First();
                if (UploadImage != null)
                {
                    byte[] buf = new byte[UploadImage.ContentLength];
                    UploadImage.InputStream.Read(buf, 0, buf.Length);
                    companiesFromDb.ImageData = buf;
                }
                companiesFromDb.CompanyName = company.CompanyName;
                companiesFromDb.CompanyAddress = company.CompanyAddress;
                companiesFromDb.CompanyCountry = company.CompanyCountry;
                companiesFromDb.CompanyCity = company.CompanyCity;
                companiesFromDb.CompanyPostalCode = company.CompanyPostalCode;
                companiesFromDb.CompanyPhoneNumber = company.CompanyPhoneNumber;
                //db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();//not tested yet
                return RedirectToAction("Company");

               

                }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Company");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[HttpPost]
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult experiencePieChart()
        {



            return View();
        }
        [Authorize(Roles = "CompanyAdministrator")]
        public JsonResult GetData()
        {
            //get all the workers in the company

            string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            Company company = db.Companies.Where(c => c.CAId == currentUserId)
                    .FirstOrDefault();
            db.Users.FirstOrDefault(x => x.Id == currentUserId);

            //IList<WorkRole> list1 = new List<WorkRole>();
            IQueryable<WorkRole> dbList = db.WorkRoles.Where(c => c.CompanyId == company.CompanyId);

            List<WorkRole> list1 = new List<WorkRole>(dbList);

            var list2 = db.WorkRoles.
                Join(db.WorkRolesUsersDetails,
                o => o.WorkRoleId, od => od.WorkRoleId,
                (o, od) => new
                {
                    WorkRoleId = o.WorkRoleId,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription,
                    CompanyId = o.CompanyId,
                    WRUDId = od.WRUDId,
                    UserDetailsId = od.UserDetailsId,
                    FocusStart = od.FocusStart,
                    FocusEnd = od.FocusEnd
                }).ToList()
                .Select(item => new RoleViewModel(
                   item.WorkRoleId,
                    item.RoleName,
                    item.RoleDescription,
                    item.CompanyId,
                    item.WRUDId,
                    item.UserDetailsId,
                    item.FocusStart,
                    item.FocusEnd)).ToList();

            var list3 = list1.
                Join(db.WorkRolesUsersDetails,
                o => o.WorkRoleId, od => od.WorkRoleId,
                (o, od) => new
                {
                    WorkRoleId = o.WorkRoleId,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription,
                    CompanyId = o.CompanyId,
                    WRUDId = od.WRUDId,
                    UserDetailsId = od.UserDetailsId,
                    FocusStart = od.FocusStart,
                    FocusEnd = od.FocusEnd
                }).ToList()
                .Select(item => new RoleViewModel(
                   item.WorkRoleId,
                    item.RoleName,
                    item.RoleDescription,
                    item.CompanyId,
                    item.WRUDId,
                    item.UserDetailsId,
                    item.FocusStart,
                    item.FocusEnd)).ToList();

            //Math.Round(3.44, 1);
            var perclist = list3.GroupBy(i => i.RoleName)
      .Select(i =>
            new {
                rolename = i.Key,
                perc = Math.Round(((double)(i.Count()) / (double)(list3.Count())) * 100, 1)
            });
            var json = JsonConvert.SerializeObject(perclist);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
