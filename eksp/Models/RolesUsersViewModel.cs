using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class RolesUsersViewModel
    {
        public RolesUsersViewModel(string userDetailsId, string FirstName, string LastName, TimeSpan totalex)
        {
            //WorkRoleId = workRoleId;
            //RoleName = roleName;
            //RoleDescription = roleDescription;
            //CompanyId = companyId;
            //WRUDId = wRUDId;
            userId = userDetailsId;
            fname = FirstName;
            lname = LastName;
            total = totalex;
        }
        //public int WorkRoleId { get; set; }
        //public string RoleName { get; set; }
        //public string RoleDescription { get; set; }
        //public int CompanyId { get; set; }
        //public int WRUDId { get; set; }
        public string userId { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        //public string UserDetailsId { get; set; }
        //public DateTime FocusStart { get; set; }
        //public DateTime FocusEnd { get; set; }
        public TimeSpan total { get; set; }

    }

   
}