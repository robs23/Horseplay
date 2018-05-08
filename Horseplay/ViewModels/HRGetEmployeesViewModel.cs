using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Horseplay.Models;
using Horseplay.DAL;

namespace Horseplay.ViewModels
{
    public class HRGetEmployeesViewModel
    {
        public HRGetEmployeesViewModel(Employee emp)
        {
            employee = emp;
            //HorseplayContext db = new HorseplayContext();
            //var db = from eg in db.EmployeeGroups
            //         where eg.EmployeeGroupId in
        }
        public Employee employee { get; set; }
        public string SelectedGroups { get; set; }

    }
}