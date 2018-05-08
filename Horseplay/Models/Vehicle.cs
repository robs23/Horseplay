using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{
    public enum VehicleType
    {
        Truck,
        Trailer,
        Car,
        Bus,
        Other
    }

    public class Vehicle
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? dateAdded { get; set; }
        [ScaffoldColumn(false)]
        public int? addedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? dateModified { get; set; }

        [ScaffoldColumn(false)]
        public int? modifiedBy { get; set; }

        [ScaffoldColumn(false)]
        public int VehicleId { get; set; }
        [Display(Name ="Numer rejestracyjny")]
        [Required(ErrorMessage ="Pole Numer rejestracyjny jest wymagane!")]
        public string Plate { get; set; }
        [Display(Name = "Marka")]
        public string Brand { get; set; }
        [Display(Name ="Model")]
        public string Model { get; set; }
        [Display(Name = "Rok produkcji")]
        public int? ProductionYear { get; set; }
        [Display(Name ="Numer VIN")]
        public string Vin { get; set; }
        [Display(Name ="Pojemność")]
        public string Capacity { get; set; }
        [Display(Name ="Rodzaj")]
        [Required(ErrorMessage ="Pole Rodzaj jest wymagane!")]
        public VehicleType Type { get; set; }
        [Display(Name ="Opis")]
        public string Description { get; set; }
        [Display(Name ="Data rejestracji")]
        public DateTime? RegistrationDate { get; set; }
        [Display(Name ="Data następnego przeglądu")]
        public DateTime? ServiceDate { get; set; }
        [Display(Name ="Domyślny użytkownik")]
        public virtual Employee DefaultUser { get; set; }
    }
}