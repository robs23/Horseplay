using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public class Route
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
        public int RouteId { get; set; }
        [Display(Name ="Długość [km]")]
        public int? Length { get; set; }
        public virtual List<RouteStop> Stops { get; set; }
    }
}