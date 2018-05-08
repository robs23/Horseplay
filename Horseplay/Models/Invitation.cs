using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public class Invitation
    {
        [ScaffoldColumn(false)]
        public int InvitationId { get; set; }
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateAdded { get; set; }
        [ScaffoldColumn(false)]
        public int? AddedBy { get; set; }
        [ScaffoldColumn(false)]
        public string InvitationToken { get; set; }
        [EmailAddress]
        [Display(Name ="Adres E-mail osoby, którą chciałbyś zaprosić")]
        [Required(ErrorMessage = "Pole Adres E-mail osoby, którą chciałbyś zaprosić jest wymagane!")]
        public  string InvitedMail { get; set; }
        [ScaffoldColumn(false)]
        public DateTime ExpirationDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? AcceptedOn { get; set; }
        [ScaffoldColumn(false)]
        public bool IsAccepted { get; set; }
    }
}