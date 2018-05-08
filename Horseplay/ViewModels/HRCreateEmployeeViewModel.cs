using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Horseplay.Models;
using System.Web.Mvc;
using Horseplay.DAL;
using Horseplay.Static;

namespace Horseplay.ViewModels
{
    public class HRCreateEmployeeViewModel
    {
        public Employee employee { get; set; }
        public IEnumerable<EmployeeGroup> employeeGroups { get; set; }
        public int? SelectedCountry { get; set; }
        public SelectList Countries(int i)
        {
            string selected = null;
            if (employee != null)
            {

            }

            return Utilities.EnumToSelectList(new Country(), selected);
        }

        public SelectList ContractTypes(int i)
        {
            string selected = null;
            if (employee != null)
            {

            }

            return Utilities.EnumToSelectList(new ContractType(), selected);
        }

        public List<int> selectedGroups { get; set; }

        public int selectedGroup { get
            {
                if (this.selectedGroups!=null){
                    return this.selectedGroups.First();
                }
                else
                {
                    return 0;
                }
                
            } }

        public ICollection<EmployeeGroup> getEmployeeGroups(List<int> selectedGroups)
        {
            HorseplayContext db = new HorseplayContext();
            List<EmployeeGroup> groups = new List<EmployeeGroup>();
            foreach (int i in selectedGroups)
            {
                var result = (from eg in db.EmployeeGroups
                              where eg.EmployeeGroupId == i
                              select eg);
                foreach (var emp in result)
                {
                    groups.Add(emp);
                }
            }
            return groups;
        }
    }
}