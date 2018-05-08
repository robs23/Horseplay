using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.ViewModels
{
    public class VehicleViewModel
    {
        public Vehicle Vehicle { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        public int? SelectedUser { get; set; }

        public SelectList VehicleTypes(int i)
        {
            string selected = null;
            if (Vehicle != null)
            {

            }

            return Utilities.EnumToSelectList(new VehicleType(), selected);
        }

        public Employee IntToEmployee(int? id)
        {
            Employee emp = null;
            if (id != null)
            {
                foreach (var p in Employees)
                {
                    if (p.EmployeeId == id)
                    {
                        emp = p;
                        break;
                    }
                }
            }
            return emp;
        }

        public void Rebuild(HorseplayContext db)
        {
            List<Employee> employees = new List<Employee>();
            int tenantId = (int)System.Web.HttpContext.Current.Session["TenantId"];
            var emps = db.Employees.Where(e => e.TenantId == tenantId);
            if (emps.Any())
            {
                foreach (var emp in emps)
                {
                    employees.Add(emp);
                }
            }
            Employees = employees;
        }
    }
}