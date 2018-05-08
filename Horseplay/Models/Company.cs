using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Horseplay.Models
{

    public enum CompanyType
    {
        Customer,
        Carrier,
        Supplier,
        Intermediary,
        Outsourcing,
        Other
    }

    public class Company
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
        public int CompanyId { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Display(Name ="Numer NIP")]
        public string TaxRegisterNumber { get; set; }
        [Display(Name ="Numer REGON")]
        public string BusinessRegisterNumber { get; set; }
        [Display(Name ="Numer KRS")]
        public string CourtRegisterNumber { get; set; }
        [Display(Name ="Termin płatności [w dniach]")]
        public int? PaymentTerm { get; set; }

        public string Name { get
            {
                if (Address != null)
                {
                    return Address.Name;
                }else
                {
                    return null;
                }
                
            } }

        public string FullName
        {
            get
            {
                if (Address != null)
                {
                    return Address.Name + ", " + Address.Street + ", " + Address.ZipCode + " " + Address.City + ", " + Address.CoutryString;
                }
                else
                {
                    return null;
                }

            }
        }

        public virtual ContactDetail ContactDetail { get; set; }
        [Display(Name ="Typ")]
        public CompanyType Type { get; set; }
    }
}