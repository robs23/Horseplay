using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Horseplay.Models
{
    public class Tenant
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [Display(Name ="Nazwa firmy")]
        public string Name { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name ="Kod pocztowy")]
        public string Code { get; set; }
        [Display(Name ="Miasto")]
        public string City { get; set; }
        [Display(Name ="Data utworzenia")]
        public DateTime? createdOn { get; set; }
        [Display(Name ="Utworzył")]
        public int createdBy { get; set; }
        
        public virtual IEnumerable<User> Users { get; set; }

    }
}