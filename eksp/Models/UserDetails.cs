using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class UserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserDetailsId { get; set; }
        public byte[] ImageData { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        [NotMapped]
        public string ImageBase64 => System.Convert.ToBase64String(ImageData);
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserAddress { get; set; }
        public string UserCountry { get; set; }
        public string UserPostalCode { get; set; }
        public string UserPhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string identtyUserId { get; set; }
        public virtual ICollection<WorkRolesUsersDetails> WorkRolesUsersDetails { get; set; }
    }
}