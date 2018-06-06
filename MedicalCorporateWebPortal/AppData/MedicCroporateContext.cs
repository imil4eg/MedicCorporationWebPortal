using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MedicalCorporateWebPortal.AppData
{
    public class MedicCroporateContext : IdentityDbContext<User, ApplicationRole, Guid>
    {
        public MedicCroporateContext(DbContextOptions<MedicCroporateContext> options) : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorProvideService> ProvideServices { get; set; }
        public DbSet<DateOfAppointment> AppointmentDates { get; set; }
        public DbSet<ReservedTime> AppoitmentReservedTime { get; set; }
        public DbSet<Specialty> Specialties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<DoctorProvideService>().ToTable("DoctorProvideService");
            modelBuilder.Entity<DateOfAppointment>().ToTable("DateOfAppointment");
            modelBuilder.Entity<ReservedTime>().ToTable("AppointmentTime");
            modelBuilder.Entity<Specialty>().ToTable("Specialty");
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}