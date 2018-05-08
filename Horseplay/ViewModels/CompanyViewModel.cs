using Horseplay.Models;
using Horseplay.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.ViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }

        public SelectList CompanyTypes(int i)
        {
            string selected = null;
            if (Company != null)
            {

            }

            return Utilities.EnumToSelectList(new CompanyType(), selected);
        }

        public SelectList Countries(int i)
        {
            string selected = null;
            if (Company != null)
            {

            }

            return Utilities.EnumToSelectList(new Country(), selected);
        }

        public int CompanyType { get; set; }
    }
}