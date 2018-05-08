using Horseplay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Horseplay.DAL
{
    public class HorseplayContext: DbContext
    {
        public HorseplayContext() : base("HorseplayConnectionString")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<CompanyGroup> CompanyGroups { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        public System.Data.Entity.DbSet<Horseplay.Models.EmployeeGroup> EmployeeGroups { get; set; }

        public System.Data.Entity.DbSet<Horseplay.Models.Employee> Employees { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<TransportOrder> TransportOrders { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasMany<EmployeeGroup>(g => g.Groups)
                .WithMany(e => e.Employees)
                .Map(m =>
                {
                    m.ToTable("EmployeesAndGroups");
                    m.MapLeftKey("EmployeeId");
                    m.MapRightKey("EmployeeGroupId");
                });
        }

        public System.Data.Entity.DbSet<Horseplay.Models.Company> Companies { get; set; }
    }
}