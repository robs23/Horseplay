using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Horseplay.Models
{
    public class Contact
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }

        [ScaffoldColumn(false)]
        public int ContactId { get; set; }
        [Required(ErrorMessage ="Pole Imię jest wymagane!")]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Pole Nazwiso jest wymagane!")]
        [Display(Name ="Nazwisko")]
        public string Surname { get; set; }
        [EmailAddress]
        [Display(Name ="E-mail")]
        public string Email { get; set; }
        [Display(Name ="Telefon")]
        public string Phone { get; set; }
        [Display(Name ="Telefon komórkowy")]
        public string Mobile { get; set; }
        [Display(Name ="Firma")]
        public int CompanyId { get; set; }
    }
}