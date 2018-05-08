using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public class User
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public int UserId { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Pole Login jest wymagane")]
        public string Login { get; set; }
        [Display(Name="Imię")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pole Imię jest wymagane")]
        public string Name { get; set; }
        [Display(Name ="Nazwisko")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pole Nazwisko jest wymagane")]
        public string Surname { get; set; }
        [EmailAddress]
        [Display(Name ="Adres E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pole E-mail jest wymagane")]
        public string Email { get; set; }
        [Display(Name ="Hasło")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pole Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Powtórz hasło")]
        [NotMapped]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pole powtórz hasło jest wymagane!")]
        [CompareAttribute("Password",ErrorMessage ="Pole hasło i powtórz hasło muszą być uzupełnione takim samym ciągiem znaków!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? createdOn { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? lastLogged { get; set; }

        [ScaffoldColumn(false)]
        public string ConfirmationToken { get; set; }
        [ScaffoldColumn(false)]
        public bool IsConfirmed { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ConfirmationDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ExpirationDate { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}