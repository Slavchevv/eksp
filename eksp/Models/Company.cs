using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eksp.Models
{
    public class Company
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CompanyId { get; set; }
        public byte[] ImageData { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        [NotMapped]
        public string ImageBase64 => System.Convert.ToBase64String(ImageData);
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CAId { get; set; }
        public virtual ICollection<WorkRole> WorkRoles { get; set; }
        public virtual ICollection<UserDetails> UserDetails { get; set; }
    }
}