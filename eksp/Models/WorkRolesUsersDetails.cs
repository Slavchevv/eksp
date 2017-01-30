using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class WorkRolesUsersDetails
    {

        [Key, Column(Order = 0)]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int WRUDId { get; set; }
        [Key, Column(Order = 1)]
        public int? WorkRoleId { get; set; }

        [Key, Column(Order = 2)]
        public string UserDetailsId { get; set; }

        
        public virtual WorkRole WorkRole { get; set; }

        public virtual UserDetails UserDetails { get; set; }

        public DateTime FocusStart { get; set; }
        public DateTime FocusEnd { get; set; }

        public bool isActive { get; set; }
    }
}