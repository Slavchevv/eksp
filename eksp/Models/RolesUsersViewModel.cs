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
            
            userId = userDetailsId;
            fname = FirstName;
            lname = LastName;
            total = totalex;
        }
       
        public string userId { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public TimeSpan total { get; set; }

    }

   
}