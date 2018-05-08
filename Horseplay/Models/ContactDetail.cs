using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public class ContactDetail
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateAdded { get; set; }
        [ScaffoldColumn(false)]
        public int? AddedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateModified { get; set; }
        [ScaffoldColumn(false)]
        public int? ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public int ContactDetailId { get; set; }
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        [Display(Name = "Komórka")]
        public string Mobile { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "E-mail")]
        public string Mail { get; set; }
        [Display(Name = "Prywatny E-mail")]
        public string PrivateMail { get; set; }
        [Display(Name = "Prywatny telefon")]
        public string PrivatePhone { get; set; }
    }
}