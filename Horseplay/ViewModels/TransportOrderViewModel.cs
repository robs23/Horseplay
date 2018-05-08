using Horseplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Horseplay.Static;
using Horseplay.DAL;
using System.Web.UI.WebControls;

namespace Horseplay.ViewModels
{
    public class TransportOrderViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Vehicle>Trailers { get; set; }

        public IEnumerable<Company> Carriers { get; set; }
        public IEnumerable<Company>Customers { get; set; }
        public TransportOrder TransportOrder { get; set; }

        public int? SelectedPrimaryDriver { get; set; }
        public int? SelectedSecondaryDriver { get; set; }
        public int? SelectedTruck { get; set; }
        public int? SelectedTrailer { get; set; }
        public int? SelectedCarrier { get; set; }
        public int? SelectedCustomer { get; set; }

        public string CoutryOptionList { get
            {
                //it builds OPTION PART Country dropdown field's string eg. <option value="0"></option><option value = "1"> Polska </option>
                int i = 0;
                string ret = "";
                foreach (var c in Enum.GetValues(typeof(Country)))
                {
                    ret += "<option value=\"" + i + "\">" + Horseplay.Static.Utilities.EnumString(new Country(), i) + "</option>";
                    i++;
                }
                return ret;
            } }

        public SelectList StopTypes(int i)
        {
            string selected = null;
            if (TransportOrder != null)
            {
                if (TransportOrder.Route.Stops.Any())
                {
                    foreach (var stop in TransportOrder.Route.Stops)
                    {
                        if (stop.Order == i)
                        {
                            selected = ((int)stop.StopType).ToString();
                            break;
                        }
                    }
                }
            }
            
            return Utilities.EnumToSelectList(new StopType(),selected);
        }

        public SelectList Countries(int i)
        {
            string selected = null;
            if (TransportOrder != null)
            {
                foreach(var stop in TransportOrder.Route.Stops)
                {
                    if (stop.Order == i)
                    {
                        selected = ((int)stop.Address.Country).ToString();
                    }
                }
            }

            return Utilities.EnumToSelectList(new Country(), selected);
        }


        public Vehicle IntToVehicle(int? id)
        {
            Vehicle prot = null;
            if (id != null)
            {
                foreach(var p in Vehicles)
                {
                    if (p.VehicleId == id)
                    {
                        prot = p;
                        break;
                    }
                }
            }
            return prot;
        }
        public Vehicle IntToTrailer(int? id)
        {
            Vehicle prot = null;
            if (id != null)
            {
                foreach (var p in Trailers)
                {
                    if (p.VehicleId == id)
                    {
                        prot = p;
                        break;
                    }
                }
            }
            return prot;
        }

        public Employee IntToEmployee(int? id)
        {
            Employee emp = null;
            if (id != null)
            {
                foreach(var p in Employees)
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

        public Company IntToCustomer(int? id)
        {
            Company comp = null;
            if (id != null)
            {
                foreach (var p in Customers)
                {
                    if (p.CompanyId == id)
                    {
                        comp = p;
                        break;
                    }
                }
            }
            return comp;
        }

        public Company IntToCarrier(int? id)
        {
            Company comp = null;
            if (id != null)
            {
                foreach (var p in Carriers)
                {
                    if (p.CompanyId == id)
                    {
                        comp = p;
                        break;
                    }
                }
            }
            return comp;
        }

        public void Rebuild(HorseplayContext db)
        {  
            int tenantId = (int)System.Web.HttpContext.Current.Session["TenantId"];
            var emps = db.Employees.Where(e => e.TenantId == tenantId);
            List<Employee> employees = new List<Employee>();

            if (emps.Any())
            {
                foreach (var emp in emps)
                {
                    employees.Add(emp);
                }
            }
            List<Vehicle> vehicles = new List<Vehicle>();
            var vehs = db.Vehicles.Where(v => v.TenantId == tenantId && (int)v.Type!=1);
            if (vehs.Any())
            {
                foreach (var veh in vehs)
                {
                    vehicles.Add(veh);
                }
            }

            List<Vehicle> trailers = new List<Vehicle>();
            var trails = db.Vehicles.Where(v => v.TenantId == tenantId && (int)v.Type==1);
            if (trails.Any())
            {
                foreach (var tr in trails)
                {
                    trailers.Add(tr);
                }
            }

            List<Company> carriers = new List<Company>();
            var cars = db.Companies.Where(v => v.TenantId == tenantId && (int)v.Type == 1);
            if (cars.Any())
            {
                foreach (var car in cars)
                {
                    carriers.Add(car);
                }
            }

            List<Company> customers = new List<Company>();
            var custs = db.Companies.Where(v => v.TenantId == tenantId && (int)v.Type == 0);
            if (custs.Any())
            {
                foreach (var cust in custs)
                {
                    customers.Add(cust);
                }
            }

            Employees = employees;
            Vehicles = vehicles;
            Trailers = trailers;
            Carriers = carriers;
            Customers = customers;
        }

    }
}