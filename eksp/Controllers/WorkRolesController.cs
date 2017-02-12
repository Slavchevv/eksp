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

            string currentUserId = User.Identity.GetUserId();
            Company comp = db.Companies.Where(c => c.CAId == currentUserId)
                    .FirstOrDefault();

            var list = db.WorkRoles.Where(c => c.CompanyId == comp.CompanyId).ToList();
            //work roles for company
            return View(list);
            //return View(db.WorkRoles.ToList());
        }

        public ActionResult DisplayListOfRolesUser()
        {
            
            string currentUserId = User.Identity.GetUserId();

            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;

            
            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();

            
            var workrolesusersdetailsQuery = db.WorkRolesUsersDetails
                .Where(m => m.UserDetailsId == currentUserId).ToList();

            
            foreach (var wu in workrolesusersdetailsQuery)
            {
                if (wu.FocusEnd > DateTime.Now)
                {
                    WorkRolesQuery.RemoveAll((x) => x.WorkRoleId == wu.WorkRoleId);
                }
            }
            
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
                .Where(m => m.UserDetailsId == currentUserId && m.isActive == true).ToList();
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
                .Where(m => m.UserDetailsId == currentUserId && m.isActive == true && m.WorkRoleId == id).FirstOrDefault();
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

        public ActionResult colleaguesInRole(int wrId, int companyId)
        {


            string currentUserId = User.Identity.GetUserId();

            UserDetails userDetails = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
                    .FirstOrDefault();

            int UsrCompanyId = userDetails.CompanyId;

            var WorkRolesQuery = db.WorkRoles.Where(c => c.CompanyId == UsrCompanyId).ToList();


            //var query = from WorkRolesUsersDetails where 
            //var workrolesusersdetailsQuery = db.WorkRolesUsersDetails.Where(m => m.UserDetailsId == currentUserId && m.isActive == true).ToList();
            //var workrolesusersdetailsQuery = db.WorkRolesUsersDetails
            //  .Where(m => m.UserDetailsId != User.Identity.GetUserId() && m.isActive == true && m.WorkRoleId==wrId);//select only thw workroleid column

            var usersInRole = db.WorkRolesUsersDetails
               .Where(m => m.UserDetailsId != currentUserId && m.isActive == true && m.WorkRoleId == wrId)
               .Select(m => m.UserDetailsId).ToList();

            //  WorkRolesQuery.RemoveAll((x) => !workrolesusersdetailsQuery.Contains(x.WorkRoleId));
            //UserDetails userDetailsView = db.UsersDetails.Where(c => c.identtyUserId == currentUserId)
            //   .ToList();
            var userProfiles = db.UsersDetails.Where(t => usersInRole.Contains(t.identtyUserId)).ToList();

            return View(userProfiles);

        }
        [HttpGet]
        public ActionResult getRolesByYear()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getRolesByYear(int year)
        {
            string currentUserId = User.Identity.GetUserId();
            //var active = db.WorkRolesUsersDetails.Where(x => x.FocusEnd.Year == year && x.UserDetailsId== currentUserId).Select(x => new { x.ContactID, x.FirstName, x.LastName });


            //var query = db.WorkRoles.Join(db.WorkRolesUsersDetails,x => x.WorkRoleId,y => y.WorkRoleId,(x, y) => new { wr = x, wrud = y }).ToList();
            //var list = db.WorkRoles.
            //    Join(db.WorkRolesUsersDetails,
            //    o => o.WorkRoleId, od => od.WorkRoleId,
            //    (o, od) => new
            //    {
            //        WorkRoleId = o.WorkRoleId,
            //        RoleName = o.RoleName,
            //        RoleDescription = o.RoleDescription,
            //        CompanyId = o.CompanyId,
            //        WRUDId = od.WRUDId,
            //        UserDetailsId = od.UserDetailsId,
            //        FocusStart = od.FocusStart,
            //        FocusEnd = od.FocusEnd
            //    }).ToList();

            //var list = db.WorkRoles.
            //Join(db.WorkRolesUsersDetails,
            //o => o.WorkRoleId, od => od.WorkRoleId,
            //(o, od) => new RoleViewModel(
            //    o.WorkRoleId,
            //    o.RoleName,
            //    o.RoleDescription,
            //    o.CompanyId,
            //    od.WRUDId,
            //    od.UserDetailsId,
            //    od.FocusStart,
            //    od.FocusEnd
            //)).ToList();

            var list = db.WorkRoles.
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


            list.RemoveAll(x => x.UserDetailsId != currentUserId);
            list = list.Where(i => i.FocusEnd.Year == year || i.FocusStart.Year == year).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_yearlyRoles", list);
            }
            else
            {
                return View(list);
            }
        }


        
        //[HttpPost]
        public ActionResult getRolesForUsersCA(int wrId)
        {
            string currentUserId = User.Identity.GetUserId();
            Company company = db.Companies.Where(c => c.CAId == currentUserId)
                    .FirstOrDefault();
            //db.Users.FirstOrDefault(x => x.Id == currentUserId);

            IQueryable<WorkRolesUsersDetails> listWRUD = db.WorkRolesUsersDetails.Where(c => c.WorkRoleId == wrId);
            //var listWRUD = db.WorkRolesUsersDetails.Where(c => c.WorkRoleId == wrId).ToList();

            var list = listWRUD.
                Join(db.UsersDetails,
                o => o.UserDetailsId, od => od.identtyUserId,
                (o, od) => new
                {
                    fname = od.FirstName,
                    lname = od.LastName,
                    UserDetailsId = o.UserDetailsId,
                    FocusStart = o.FocusStart,
                    FocusEnd = o.FocusEnd
                }).ToList();
          
            var a = from x in list
            group x by new { x.fname, x.lname, x.UserDetailsId } into g
            select new RolesUsersViewModel(g.Key.UserDetailsId, g.Key.fname, g.Key.lname, TimeSpan.FromMilliseconds(g.Sum(x => (x.FocusEnd - x.FocusStart).TotalMilliseconds)));
            List<RolesUsersViewModel> list_users = a.ToList<RolesUsersViewModel>();
            //make and if, so it returns an empty view for if there is no-one in a role. also there is some bug with time
            return View(list_users);
            
        }
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult аllUsers()
        {
            //get all users from the company
            //display them in a basic way
            //have an action link See focuses on each employee

            string currentUserId = User.Identity.GetUserId();
            Company company = db.Companies.Where(c => c.CAId == currentUserId).FirstOrDefault();
            var userProfiles = db.UsersDetails.Where(t => t.CompanyId == company.CompanyId).ToList();
          
            return View(userProfiles);
            //
        }



        //
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult focusedRoles(string usrId)
        {

            //string currentUserId = User.Identity.GetUserId();
            var list = db.WorkRoles.
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


            list.RemoveAll(x => x.UserDetailsId != usrId);
            //list = list.Where(i => i.FocusEnd.Year == year || i.FocusStart.Year == year).ToList();
            return View(list);
        }
        //

    }
}
