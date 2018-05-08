using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Horseplay.DAL;

namespace Horseplay.Models
{
    public class Employee
    {
        [ScaffoldColumn(false)]
        public int TenantId { get; set; }
        [ScaffoldColumn(false)]
        public int EmployeeId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name="Dodano")]
        public DateTime? dateAdded { get; set; }
        [ScaffoldColumn(false)]
        public int? addedBy { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Zmodyfikowano")]
        public DateTime? dateModified { get; set; }
        [ScaffoldColumn(false)]
        public int? modifiedBy { get; set; }
        [Display(Name ="Imię")]
        [Required(ErrorMessage ="Pole Imię jest obowiązkowe!")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole Nazwisko jest obowiązkowe!")]
        public string Surname { get; set; }
        [Display(Name ="Grupa")]
        public int? EmployeeGroupId { get; set; }
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDay { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name ="Data zatrudnienia")]
        public DateTime? EmployedOn { get; set; }
        [Display(Name ="Telefon")]
        public string Phone { get; set; }
        [Display(Name ="Komórka")]
        public string Mobile { get; set; }
        [Display(Name ="E-mail")]
        [EmailAddress]
        public string Mail { get; set; }
        [Display(Name ="Adres")]
        public string Address { get; set; }
        [NotMapped]
        [Display(Name="Grupy")]
        public string SelectedGroups
        { get
            {
                string val = "";
                if (this.Groups != null)
                {
                    if (this.Groups.Any())
                    {
                        foreach (var g in this.Groups)
                        {
                            val = val + g.Name + ", ";
                        }
                        if (val.Length > 0)
                        {
                            val = val.Substring(0, val.Length - 2);
                        }
                    }
                }
                
                return val;
            } }
        [NotMapped]
        [Display(Name="Imię i nazwisko")]
        public string FullName { get
            {
                string val = "";
                val = this.Name + ' ' + this.Surname;
                return val;
            } }
        public bool BelongsToGroup(int groupId)
        {
            bool test = false;
            foreach(var item in this.Groups)
            {
                if (item.EmployeeGroupId == groupId)
                {
                    test = true;
                    break;
                }
            }
            return test;
        }

        public virtual ICollection<EmployeeGroup> Groups {get; set; }

        [Display(Name ="PESEL")]
        public string PersonalIdentityNumber { get; set; }
        [Display(Name ="Kraj pochodzenia")]
        [Required(ErrorMessage ="Pole Kraj pochodzenia jest wymagane!")]
        public Country Country { get; set; }
        [Display(Name="Numer dowodu")]
        public string IdNumber { get; set; }
        [Display(Name ="Rodzaj umowy")]
        [Required(ErrorMessage ="Pole Rodzaj umowy jest wymagane!")]
        public ContractType ContractType { get; set; }
        [Display(Name ="Data wygaśnięcia")]
        public DateTime? ExpirationDate { get; set; }

    }

    public enum ContractType
    {
        Pernament,
        Temporary,
        Service,
        B2B,
        Other
    }

    public enum Country
    {
        NotSet,
        PL,
        UA,
        LT,
        LV,
        EE,
        DE,
        BY,
        RU,
        SK,
        SI,
        CZ,
        RO,
        HU,
        DK,
        NL,
        FR,
        AT,
        BE,
        IT,
        ES,
        PT,
        AL,
        AM,
        BA,
        BG,
        CH,
        EL,
        FI,
        GE,
        HR,
        IE,
        LI,
        LU,
        MC,
        MD,
        ME,
        MT,
        NO,
        RS,
        SE,
        SM,
        UK,
        MK,
        TR
    }
}