using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class WorkRole
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int WorkRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<WorkRolesUsersDetails> WorkRolesUsersDetails { get; set; }
    }
}