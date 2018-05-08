using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public class RouteStop
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
        public int RouteStopId { get; set; }
        [Display(Name ="Data przyjazdu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ArrivalDate { get; set; }

        [Display(Name = "Data wyjazdu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }
        [Display(Name ="Lp")]
        public StopType StopType { get; set; }
        public int Order { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        public int? RouteId { get; set; }
        public virtual Route Route { get; set; }

        public string ArrivalDateShort { get
            {
                if (ArrivalDate != null)
                {
                    return ArrivalDate.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    return "";
                }
            } }

        public string DepartureDateShort
        {
            get
            {
                if (DepartureDate != null)
                {
                    return DepartureDate.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
    }


    public enum StopType
    {
        loading,
        unloading,
        parking,
        other
    }
}