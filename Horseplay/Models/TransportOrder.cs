using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public enum TransportType
    {
        Own,
        Rented
    }

    public class TransportOrder
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
        public int TransportOrderId { get; set; }
        [Display(Name ="Stawka")]
        [DataType(DataType.Currency)]
        public float? Cost { get; set; }
        [Display(Name ="Dodatkowe informacje")]
        public string Remarks { get; set; }
        [Display(Name ="Typ transportu")]
        [Required(ErrorMessage ="Pole Typ transportu jest wymagane!")]
        public TransportType Type { get; set; }
        [Display(Name ="Ciągnik")]
        public virtual Vehicle Truck { get; set; }
        [Display(Name ="Naczepa")]
        public virtual Vehicle Trailer { get; set; }
        [Display(Name ="Kierowca")]
        public virtual Employee PrimaryDriver { get; set; }
        [Display(Name ="Kierowca pomocniczy")]
        public virtual Employee SecondaryDriver { get; set; }
        [Display(Name ="Przewoźnik")]
        public virtual Company Carrier { get; set; }
        [Display(Name = "Klient")]
        public virtual Company Customer { get; set; }
        public int? RouteId { get; set; }
        public virtual Route Route { get; set; }

        
    }
}