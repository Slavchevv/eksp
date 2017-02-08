using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class RoleViewModel
    {
        public RoleViewModel(int workRoleId, string roleName, string roleDescription, int companyId, int wRUDId, string userDetailsId, DateTime focusStart, DateTime focusEnd)
        {
            WorkRoleId = workRoleId;
            RoleName = roleName;
            RoleDescription = roleDescription;
            CompanyId = companyId;
            WRUDId = wRUDId;
            UserDetailsId = userDetailsId;
            FocusStart = focusStart;
            FocusEnd = focusEnd;
        }
        public int WorkRoleId { get; set; }
        public string RoleName  { get; set; }
        public string RoleDescription { get; set; }
        public int CompanyId { get; set; }
        public int WRUDId { get; set; }
        public string UserDetailsId { get; set; }
        public DateTime FocusStart { get; set; }
        public DateTime FocusEnd { get; set; }

      

        


    }
}