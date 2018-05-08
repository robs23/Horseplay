using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Horseplay.Static;

namespace Horseplay.Models
{
    public class Address
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
        public int AddressId { get; set; }
        [Display(Name ="Nazwa firmy")]
        [Required(ErrorMessage ="Pole Nazwa firmy jest wymagane!")]
        public string Name { get; set; }
        [Display(Name ="Ulica i numer domu")]
        public string Street { get; set; }
        [Display(Name ="Kod pocztowy")]
        public string ZipCode { get; set; }
        [Display(Name ="Miasto")]
        public string City { get; set; }
        [Display(Name = "Województwo/Region")]
        public string Province { get; set; }
        [Display(Name ="Kraj")]
        public Country Country { get; set; }
        [NotMapped]
        public string FullName { get
            {
                string str="";
                str = Name;
                if (City !=null || ZipCode !=null)
                {
                    str = str + ", " + ZipCode + ' ' + City;
                }
                if (Country != Country.NotSet)
                {
                    str = str + ", " + CoutryString;
                }
                return str;
            } }

        public string CoutryString { get
            {
                return Utilities.EnumString(new Country(), (int)Country);
            } }
    }
}