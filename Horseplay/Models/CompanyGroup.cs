using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Horseplay.Models
{
    public class CompanyGroup
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public int CompanyGroupId { get; set; }
        [Required(ErrorMessage ="Nazwa grupy jest wymagana!")]
        [Display(Name="Nazwa grupy")]       
        public string Name { get; set; }
        [Display(Name="Opis grupy")]
        public string Description { get; set; }

    }
}