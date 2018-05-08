using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.Controllers
{
    public class LogisticsController : Controller
    {
        // GET: Logistics
        public ActionResult Index()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult CreateTransportOrder()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                TransportOrderViewModel vm = new TransportOrderViewModel();
                vm = FillTransportOrderViewModel(vm);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult CreateTransportOrder(TransportOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = FillTransportOrderViewModel(vm);
                return View("CreateTransportOrder", vm);
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                vm.Rebuild(db);
                if (vm.TransportOrder.Route.Stops.Any())
                {
                    int userId = (int)Session["UserId"];
                    int tenantId = (int)Session["TenantId"];
                    int order = 0;
                    //if there are any stops, let's create them
                    List<RouteStop> stops = new List<RouteStop>();
                    foreach (var stop in vm.TransportOrder.Route.Stops)
                    {
                        Address address = new Address();
                        address.Name = stop.Address.Name;
                        address.Street = stop.Address.Street;
                        address.ZipCode = stop.Address.ZipCode;
                        address.City = stop.Address.City;
                        address.Country = stop.Address.Country;
                        address.AddedBy = userId;
                        address.TenantId = tenantId;
                        address.DateAdded = DateTime.Now;
                        RouteStop rs = new RouteStop();
                        rs.Address = address;
                        rs.AddedBy = userId;
                        rs.DepartureDate = stop.DepartureDate;
                        rs.ArrivalDate = stop.ArrivalDate;
                        rs.StopType = stop.StopType;
                        rs.TenantId = tenantId;
                        rs.DateAdded = DateTime.Now;
                        rs.Order = order;
                        stops.Add(rs);
                        order++;
                    }
                    Route Route = new Route() { AddedBy = userId, TenantId = tenantId, DateAdded = DateTime.Now, Length=vm.TransportOrder.Route.Length, Stops = stops };
                    TransportOrder to = new TransportOrder() { AddedBy = userId, DateAdded = DateTime.Now, TenantId = tenantId, Type = vm.TransportOrder.Type, Route = Route, Cost=vm.TransportOrder.Cost, Truck=vm.IntToVehicle(vm.SelectedTruck), Trailer=vm.IntToVehicle(vm.SelectedTrailer), PrimaryDriver=vm.IntToEmployee(vm.SelectedPrimaryDriver), SecondaryDriver=vm.IntToEmployee(vm.SelectedSecondaryDriver), Remarks=vm.TransportOrder.Remarks, Carrier=vm.IntToCarrier(vm.SelectedCarrier), Customer=vm.IntToCustomer(vm.SelectedCustomer) };
                    db.TransportOrders.Add(to);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var error in e.EntityValidationErrors)
                        {
                            foreach (var propertyError in error.ValidationErrors)
                            {
                                Console.WriteLine($"{propertyError.PropertyName} had the following issue: {propertyError.ErrorMessage}");
                            }
                        }
                    }

                }
                return RedirectToAction("GetTransportOrders");
            }

        }

        public ActionResult GetTransportOrders()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                var tos = db.TransportOrders
                            .Include(to => to.Truck)
                            .Include(to => to.Trailer)
                            .Include(to => to.PrimaryDriver)
                            .Include(to => to.SecondaryDriver)
                            .Include(to => to.Carrier)
                            .Include(to => to.Carrier.Address)
                            .Include(to => to.Customer)
                            .Include(to => to.Route.Stops.Select(s => s.Address))
                            .Where(t => t.TenantId == tenantId).OrderByDescending(t => t.DateAdded);
                return View(tos);
            }
 
        }

        public ActionResult TransportOrderDetails(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var tos = db.TransportOrders
                            .Include(to => to.Truck)
                            .Include(to => to.Trailer)
                            .Include(to => to.PrimaryDriver)
                            .Include(to => to.SecondaryDriver)
                            .Include(to => to.Carrier)
                            .Include(to => to.Customer)
                            .Include(to => to.Route.Stops.Select(s => s.Address))
                            .Where(t => t.TenantId == tenantId && t.TransportOrderId == id);
                if (tos.Any())
                {
                    return View(tos.FirstOrDefault());

                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        public ActionResult DeleteTransportOrder(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var tos = db.TransportOrders
                            .Include(to => to.Truck)
                            .Include(to => to.Trailer)
                            .Include(to => to.PrimaryDriver)
                            .Include(to => to.SecondaryDriver)
                            .Include(to => to.Carrier)
                            .Include(to => to.Customer)
                            .Include(to => to.Route.Stops.Select(s => s.Address))
                            .Where(t => t.TenantId == tenantId && t.TransportOrderId == id);
                if (tos.Any())
                {
                    return View(tos.FirstOrDefault());

                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }
        [HttpPost]
        public ActionResult DeleteTransportOrder(TransportOrder to)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["Tenantid"];
            var tos = db.TransportOrders
                        .Include(x => x.Route)
                        .Include(x=>x.Route.Stops)
                        .Include(x => x.Route.Stops.Select(s => s.Address))
                        .Where(x => x.TransportOrderId == to.TransportOrderId && x.TenantId == tenantId);
            if (tos.Any())
            {
                TransportOrder oldTo = tos.FirstOrDefault();
                DeleteDependentRoute(oldTo, db);
                db.TransportOrders.Remove(oldTo);
                db.SaveChanges();
                return RedirectToAction("GetTransportOrders");
            }
            else
            {
                return new HttpNotFoundResult("Nieoczekiwany błąd");
            }
        }

        public ActionResult EditTransportOrder(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                TransportOrderViewModel vm = new TransportOrderViewModel();
                vm = FillTransportOrderViewModel(vm, id);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult EditTransportOrder(TransportOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = FillTransportOrderViewModel(vm);
                return View("EditTransportOrder", vm);
            }
            else
            {
                int userId = (int)Session["UserId"];
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                vm.Rebuild(db);
                var tos = db.TransportOrders.Where(x => x.TenantId == tenantId && x.TransportOrderId == vm.TransportOrder.TransportOrderId);
                TransportOrder oldTo = tos.FirstOrDefault();
                DeleteDependentRoute(oldTo, db);
                if (vm.TransportOrder.Route.Stops.Any())
                {
                    
                    int order = 0;
                    //if there are any stops, let's create them
                    List<RouteStop> stops = new List<RouteStop>();
                    foreach (var stop in vm.TransportOrder.Route.Stops)
                    {
                        Address address = new Address();
                        address.Name = stop.Address.Name;
                        address.Street = stop.Address.Street;
                        address.ZipCode = stop.Address.ZipCode;
                        address.City = stop.Address.City;
                        address.Country = stop.Address.Country;
                        address.AddedBy = userId;
                        address.TenantId = tenantId;
                        address.DateAdded = DateTime.Now;
                        RouteStop rs = new RouteStop();
                        rs.Address = address;
                        rs.AddedBy = userId;
                        rs.DepartureDate = stop.DepartureDate;
                        rs.ArrivalDate = stop.ArrivalDate;
                        rs.StopType = stop.StopType;
                        rs.TenantId = tenantId;
                        rs.DateAdded = DateTime.Now;
                        rs.Order = order;
                        stops.Add(rs);
                        order++;
                    }
                    Route Route = new Route() { AddedBy = userId, TenantId = tenantId, DateAdded = DateTime.Now, Length = vm.TransportOrder.Route.Length, Stops = stops };
                    oldTo.Type = vm.TransportOrder.Type;
                    oldTo.ModifiedBy = userId;
                    oldTo.DateModified = DateTime.Now;
                    oldTo.Truck = vm.IntToVehicle(vm.SelectedTruck);
                    oldTo.Trailer = vm.IntToTrailer(vm.SelectedTrailer);
                    oldTo.PrimaryDriver = vm.IntToEmployee(vm.SelectedPrimaryDriver);
                    oldTo.SecondaryDriver = vm.IntToEmployee(vm.SelectedSecondaryDriver);
                    oldTo.Carrier = vm.IntToCarrier(vm.SelectedCarrier);
                    oldTo.Customer = vm.IntToCustomer(vm.SelectedCustomer);
                    oldTo.Cost = vm.TransportOrder.Cost; 
                    oldTo.Remarks = vm.TransportOrder.Remarks;
                    oldTo.Route = Route;
                    db.Entry(oldTo).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var error in e.EntityValidationErrors)
                        {
                            foreach (var propertyError in error.ValidationErrors)
                            {
                                Console.WriteLine($"{propertyError.PropertyName} had the following issue: {propertyError.ErrorMessage}");
                            }
                        }
                    }

                }
                return RedirectToAction("GetTransportOrders");
            }
        }

        public ActionResult GetVehicles()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var vehicles = db.Vehicles.Where(v => v.TenantId == tenantId);
                return View(vehicles);
            }
        }

        public ActionResult CreateVehicle()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                VehicleViewModel vm = new VehicleViewModel();
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                vm.Rebuild(db);
                return View(vm);
            }
        }

        [HttpPost]

        public ActionResult CreateVehicle(VehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateVehicle", vm);
            }
            else
            {
                string plate = vm.Vehicle.Plate.Trim();
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                if (plate.Length > 0)
                {
                    if (db.Vehicles.Any(v => v.Plate == plate && v.TenantId == tenantId))
                    {
                        ModelState.AddModelError(string.Empty, "Pojazd o takiej tablicy rejestracyjnej już istnieje. Numer rejestracyjny musi być unikalny");
                        return View("CreateVehicle", vm);
                    }
                }
                vm.Rebuild(db);
                //all is fine. Let's create this motherfucker
                Vehicle Vehicle = new Vehicle();
                Vehicle.Plate = plate;
                Vehicle.Brand = vm.Vehicle.Brand;
                Vehicle.Model = vm.Vehicle.Model;
                Vehicle.ProductionYear = vm.Vehicle.ProductionYear;
                Vehicle.Capacity = vm.Vehicle.Capacity;
                Vehicle.RegistrationDate = vm.Vehicle.RegistrationDate;
                Vehicle.ServiceDate = vm.Vehicle.ServiceDate;
                Vehicle.Type = vm.Vehicle.Type;
                Vehicle.Vin = vm.Vehicle.Vin;
                Vehicle.DefaultUser = vm.IntToEmployee(vm.SelectedUser);
                Vehicle.TenantId = tenantId;
                Vehicle.addedBy = (int)Session["UserId"];
                Vehicle.dateAdded = DateTime.Now;
                db.Vehicles.Add(Vehicle);
                db.SaveChanges();
                return RedirectToAction("GetVehicles");
            }
        }

        public ActionResult VehicleDetails(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var veh = db.Vehicles
                    .Include(v => v.DefaultUser)
                    .Where(v => v.TenantId == tenantId && v.VehicleId == id);
                if (veh.Any())
                {
                    return View("VehicleDetails", veh.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        public ActionResult EditVehicle(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                VehicleViewModel vm = new VehicleViewModel();
                int tenantId = (int)Session["TenantId"];
                var veh = db.Vehicles.Where(v => v.TenantId == tenantId && v.VehicleId == id);
                if (veh.Any())
                {
                    vm.Rebuild(db);
                    vm.Vehicle = veh.FirstOrDefault();
                    return View("EditVehicle", vm);
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult EditVehicle(VehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("EditVehicle", vm);
            }else
            {
                HorseplayContext db = new HorseplayContext();
                vm.Rebuild(db);
                string plate = vm.Vehicle.Plate.Trim();
                if (db.Vehicles.Any(v=>v.Plate ==plate && v.VehicleId != vm.Vehicle.VehicleId)){
                    ModelState.AddModelError(string.Empty, "Pojazd o takiej tablicy rejestracyjnej już istnieje. Numer rejestracyjny musi być unikalny");
                    return View("EditVehicle", vm);
                }
                else
                {
                    int tenantId = (int)Session["TenantId"];
                    var vehs = db.Vehicles.Where(v => v.VehicleId == vm.Vehicle.VehicleId && v.TenantId == tenantId);
                    if (vehs.Any())
                    {
                        Vehicle oldVeh = vehs.FirstOrDefault();
                        oldVeh.dateModified = DateTime.Now;
                        oldVeh.modifiedBy = (int)Session["UserId"];
                        oldVeh.Plate = plate;
                        oldVeh.Brand = vm.Vehicle.Brand;
                        oldVeh.Model = vm.Vehicle.Model;
                        oldVeh.ProductionYear = vm.Vehicle.ProductionYear;
                        oldVeh.Capacity = vm.Vehicle.Capacity;
                        oldVeh.RegistrationDate = vm.Vehicle.RegistrationDate;
                        oldVeh.ServiceDate = vm.Vehicle.ServiceDate;
                        oldVeh.Type = vm.Vehicle.Type;
                        oldVeh.Vin = vm.Vehicle.Vin;
                        oldVeh.DefaultUser = vm.IntToEmployee(vm.SelectedUser);
                        oldVeh.Description = vm.Vehicle.Description;
                        db.Entry(oldVeh).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("GetVehicles");
                    }
                    else
                    {
                        return new HttpNotFoundResult("Nie oczekiwany błąd");
                    }
                }
            }
        }

        public ActionResult DeleteVehicle(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int tenantId = (int)Session["Tenantid"];
                HorseplayContext db = new HorseplayContext();
                var veh = db.Vehicles.Where(v => v.VehicleId == id && v.TenantId == tenantId);
                if (veh.Any())
                {
                    return View("DeleteVehicle", veh.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }
        [HttpPost]
        public ActionResult DeleteVehicle(Vehicle veh)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["Tenantid"];
            var vehs = db.Vehicles.Where(v => v.VehicleId == veh.VehicleId && v.TenantId == tenantId);
            if (vehs.Any())
            {
                Vehicle oldVeh = vehs.FirstOrDefault();
                db.Vehicles.Remove(oldVeh);
                db.SaveChanges();
                return RedirectToAction("GetVehicles");
            }else
            {
                return new HttpNotFoundResult("Nieoczekiwany błąd");
            }
        }

        public TransportOrderViewModel FillTransportOrderViewModel(TransportOrderViewModel vm, int id = 0)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
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
            var trails = db.Vehicles.Where(v => v.TenantId == tenantId && (int)v.Type == 1);
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

            vm.Employees = employees;
            vm.Vehicles = vehicles;
            vm.Trailers = trailers;
            vm.Carriers = carriers;
            vm.Customers = customers;

            if (id>0)
            {
                var tos = db.TransportOrders
                        .Include(to => to.Truck)
                        .Include(to => to.Trailer)
                        .Include(to => to.PrimaryDriver)
                        .Include(to => to.SecondaryDriver)
                        .Include(to => to.Carrier)
                        .Include(to=>to.Customer)
                        .Include(to=>to.Route)
                        .Include(to=>to.Route.Stops)
                        .Include(to => to.Route.Stops.Select(s => s.Address))
                        .Where(t => t.TenantId == tenantId && t.TransportOrderId == id);

                if (tos.Any())
                {
                    vm.TransportOrder = tos.FirstOrDefault();
                    if (vm.TransportOrder.Carrier != null)
                    {
                        vm.SelectedCarrier = vm.TransportOrder.Carrier.CompanyId;
                    }
                    if (vm.TransportOrder.Customer != null)
                    {
                        vm.SelectedCustomer = vm.TransportOrder.Customer.CompanyId;
                    }
                    if (vm.TransportOrder.PrimaryDriver != null)
                    {
                        vm.SelectedPrimaryDriver = vm.TransportOrder.PrimaryDriver.EmployeeId;
                    }
                    if (vm.TransportOrder.SecondaryDriver != null)
                    {
                        vm.SelectedSecondaryDriver = vm.TransportOrder.SecondaryDriver.EmployeeId;
                    }
                    if (vm.TransportOrder.Truck != null)
                    {
                        vm.SelectedTruck = vm.TransportOrder.Truck.VehicleId;
                    }
                    if(vm.TransportOrder.Trailer != null)
                    {
                        vm.SelectedTrailer = vm.TransportOrder.Trailer.VehicleId;
                    }
                }
            }

            return vm;
        }

        public void DeleteDependentRoute(TransportOrder to, HorseplayContext db)
        {
            if (to.Route != null)
            {
                //delete route data
                if (to.Route.Stops.Any())
                {
                    Address oldAddress;
                    RouteStop oldStop;
                    List<int> stopIds = new List<int>();
                    //we have to delete all the stops first
                    foreach (var stop in to.Route.Stops)
                    {
                        var ads = db.Addresses.Where(a => a.AddressId == stop.AddressId);
                        if (ads.Any())
                        {
                            oldAddress = stop.Address;
                            db.Addresses.Remove(oldAddress);
                            db.SaveChanges();
                        }
                        stopIds.Add(stop.RouteStopId);
                    }
                    foreach(int i in stopIds)
                    {
                        oldStop = db.RouteStops.Where(rs => rs.RouteStopId == i).FirstOrDefault();
                        db.RouteStops.Remove(oldStop);
                        db.SaveChanges();
                    }
                }
                Route oldRoute = db.Routes.Where(r => r.RouteId == to.Route.RouteId).FirstOrDefault();
                db.Routes.Remove(oldRoute);
                db.SaveChanges();
            }
        }

        public JsonResult SearchAddress(string Prefix)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
            var addresses = db.Addresses.Where(a => a.TenantId == tenantId && (a.Name.Contains(Prefix))).Select(y => new { y.Name, y.Street, y.ZipCode, y.City, y.Country }).Distinct().ToList();

            return Json(addresses, JsonRequestBehavior.AllowGet);
        }

        public string GetCountryOptionList()
        {
            //it builds OPTION PART Country dropdown field's string eg. <option value="0"></option><option value = "1"> Polska </option>
            int i = 0;
            string ret = "";
            foreach(var c in Enum.GetValues(typeof(Country)))
            {
                ret += "<option value=\"" + i + "\">" + Horseplay.Static.Utilities.EnumString(new Country(), i) + "</option>";
                i++;
            }
            return ret;
        }
    }
}