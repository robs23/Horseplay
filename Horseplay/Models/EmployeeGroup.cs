using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Horseplay.Models
{
    public class EmployeeGroup
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }

        [ScaffoldColumn(false)]
        public int EmployeeGroupId { get; set; }

        [Display(Name ="Nazwa grupy")]
        [Required(ErrorMessage ="Nazwa grupy jest wymagana!")]
        public string Name { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name="Dodano")]
        public DateTime? dateAdded { get; set; }
        [ScaffoldColumn(false)]
        public int? addedBy { get; set; }
        [Display(Name="Zmodyfikowano")]
        [ScaffoldColumn(false)]
        public DateTime? dateModified { get; set; }
        [ScaffoldColumn(false)]
        public int? modifiedBy { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}