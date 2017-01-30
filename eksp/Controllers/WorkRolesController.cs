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
using System.Globalization;

namespace eksp.Controllers
{
    //[Authorize(Roles = "CompanyAdministrator, Employee")]
    public class WorkRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkRoles
        public ActionResult Index()
        {
            return View(db.WorkRoles.ToList());
        }

        public ActionResult DisplayListOfRolesUser()
        {
            //get current user id
            //query the users table and get the user's company
            //display a list of roles for that company
            string currentUserId = User.Identity.GetUserId();
           
            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;


            //
            //var list = new List<WorkRole>();
            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();

            //check if some of the roles match with those the user already is 'focused' on
            //if there are such remove them from the list and then pass it to the view
            var workrolesusersdetailsQuery = db.WorkRolesUsersDetails
                .Where(m => m.UserDetailsId == currentUserId).ToList();

            //foreach (var wr in WorkRolesQuery)
            //{
                //if end date bigger then current date
                //then remove the role/s in WorkRolesQuery with the same ids
                foreach (var wu in workrolesusersdetailsQuery) { 
                    if(wu.FocusEnd> DateTime.Now)
                    {
                        WorkRolesQuery.RemoveAll((x) => x.WorkRoleId == wu.WorkRoleId);
                    }
                }
            //}

            //for (int i = 0; i < WorkRolesQuery.Count; i++)
            //{
            //    //Category cat = categoryList[index];
            //}

            return View(WorkRolesQuery);
            
        }

        public ActionResult DisplayListOfFocusedRolesUser()
        {
            //get current user id
            //query the users table and get the user's company
            //display a list of roles for that company
            string currentUserId = User.Identity.GetUserId();

            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;

            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();


            //var query = from WorkRolesUsersDetails where 
            //var workrolesusersdetailsQuery = db.WorkRolesUsersDetails.Where(m => m.UserDetailsId == currentUserId && m.isActive == true).ToList();
            var workrolesusersdetailsQuery = db.WorkRolesUsersDetails
                .Where(m => m.UserDetailsId == currentUserId && m.isActive == true)
                .Select(m => m.WorkRoleId).ToList();//select only thw workroleid column

            //foreach (var wr in workrolesusersdetailsQuery)
            //{
            //    var temp = wr;
            //    WorkRolesQuery.RemoveAll((x) => x.WorkRoleId != wr);
            //}

            WorkRolesQuery.RemoveAll((x) => !workrolesusersdetailsQuery.Contains(x.WorkRoleId));


            return View(WorkRolesQuery);

        }

        public ActionResult addWorkRoleUser(int? id)//change to normal method
        {

            int? roleid = id;
            //on button click check in the db if a user doesn't already have three assigned roles for one year back.
            //if the user doesn't then insert the choosen role id and the user id into the database 
            //with the dates as well current date + one year ahead as an end date
            //дд
            DateTime date = DateTime.Now;
            var WorkRoleUser = new WorkRolesUsersDetails
            {
                WorkRoleId = id,
                UserDetailsId = User.Identity.GetUserId(),
                FocusStart = date,
                FocusEnd = date.AddYears(1),
                isActive = true

        };

            db.WorkRolesUsersDetails.Add(WorkRoleUser); 
            db.SaveChanges();

            return RedirectToAction("DisplayListOfRolesUser");

            //return View();

        }

        // GET: WorkRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkRole workRole = db.WorkRoles.Find(id);
            if (workRole == null)
            {
                return HttpNotFound();
            }
            return View(workRole);
        }

        // GET: WorkRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkRoleId,RoleName,RoleDescription")] WorkRole workRole)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();

                Company company = db.Companies.Where(c => c.CAId == currentUserId)
                       .FirstOrDefault();
                int companyID = company.CompanyId;

                workRole.CompanyId = companyID;
           
                db.WorkRoles.Add(workRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workRole);
        }

        // GET: WorkRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkRole workRole = db.WorkRoles.Find(id);
            if (workRole == null)
            {
                return HttpNotFound();
            }
            return View(workRole);
        }

        // POST: WorkRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkRoleId,RoleName,RoleDescription")] WorkRole workRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workRole);
        }

        // GET: WorkRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkRole workRole = db.WorkRoles.Find(id);
            if (workRole == null)
            {
                return HttpNotFound();
            }
            return View(workRole);
        }

        // POST: WorkRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkRole workRole = db.WorkRoles.Find(id);
            db.WorkRoles.Remove(workRole);
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

        [HttpPost]
        public ActionResult checkNumRoles()
        {
            //check if the user already has three choosen roles (active)
            //if yes return false
            //if no return true
            string currentUserId = User.Identity.GetUserId();

            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;


            //
            //var list = new List<WorkRole>();
            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();

            //check if some of the roles match with those the user already is 'focused' on
            //if there are such remove them from the list and then pass it to the view
            var workrolesusersdetailsQuery = db.WorkRolesUsersDetails
                .Where(m => m.UserDetailsId == currentUserId && m.isActive==true).ToList();
            var rolesCount = workrolesusersdetailsQuery.Count;
            //foreach (var wu in workrolesusersdetailsQuery)
            //{
            //    if (wu.FocusEnd > DateTime.Now)
            //    {
            //        WorkRolesQuery.RemoveAll((x) => x.WorkRoleId == wu.WorkRoleId);
            //    }
            if (rolesCount == 3)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
                
        }

        //
       
        public ActionResult unfocusFromRole(int? id)
        {
            //where role id and user id are equal to the ones passed as parameters
            //update isActive to false, update end date with the current date
            //int? roleid = id;


            string currentUserId = User.Identity.GetUserId();

            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;


           
            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();

            //check if some of the roles match with those the user already is 'focused' on
            //if there are such remove them from the list and then pass it to the view
            WorkRolesUsersDetails workrolesusersdetailsQuery = db.WorkRolesUsersDetails
                .Where(m => m.UserDetailsId == currentUserId && m.isActive == true && m.WorkRoleId==id).FirstOrDefault();
            //var rolesCount = workrolesusersdetailsQuery.Count;
            workrolesusersdetailsQuery.isActive = false;
            workrolesusersdetailsQuery.FocusEnd = DateTime.Now;
            db.Entry(workrolesusersdetailsQuery).State = EntityState.Modified;
            db.SaveChanges();

            //foreach (var wu in workrolesusersdetailsQuery)
            //{
            //    if (wu.FocusEnd > DateTime.Now)
            //    {
            //        WorkRolesQuery.RemoveAll((x) => x.WorkRoleId == wu.WorkRoleId);
            //    }
            return RedirectToAction("Index");

        }
        //
        public void addPastRole(int wrId, string dateStart, string dateEnd)
        {
            //Some code
            

            DateTime dtStart = DateTime.ParseExact(dateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtEnd = DateTime.ParseExact(dateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var WorkRoleUser = new WorkRolesUsersDetails
            {
                WorkRoleId = wrId,
                UserDetailsId = User.Identity.GetUserId(),
                FocusStart = dtStart,
                FocusEnd = dtEnd.AddYears(1),
                isActive = false

            };

            db.WorkRolesUsersDetails.Add(WorkRoleUser);
            db.SaveChanges();
           


            //return Json(str);
        }
    }
}
