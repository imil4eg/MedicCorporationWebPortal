using MedicalCorporateWebPortal.Models;
using System;
using System.Linq;

namespace MedicalCorporateWebPortal.AppData
{
    public static class DbInitializer
    {
        public static void Initialize(MedicCroporateContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User{Login = "qwerty", Password = "qwerty", LastName = "Пупкин", FirstName = "Вася", MiddleName = "Васильевич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new User{Login = "qwerty1", Password = "qwerty1", LastName = "Васильев", FirstName = "Вася", MiddleName = "Васильевич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new User{Login = "qwerty2", Password = "qwerty2", LastName = "Ильин", FirstName = "Илья", MiddleName = "Ильич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new User{Login = "qwerty3", Password = "qwerty3", LastName = "Петяев", FirstName = "Петя", MiddleName = "Петькович", Gender = Gender.Мужской, Role = UserRole.Врач}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }

            context.SaveChanges();

            for (int i = 1; i < users.Length + 1; i++)
            {
                context.Employees.Add(new Employee { UserID = users[i - 1].UserID });
            }

            context.SaveChanges();

            var doctors = new Doctor[]
            {
                new Doctor{EmployeeID = 1, Speciality = "Хирург"},
                new Doctor{EmployeeID = 2, Speciality = "Косметолог"},
                new Doctor{EmployeeID = 3, Speciality = "Физиотерапевт" },
                new Doctor{EmployeeID = 4, Speciality = "Ревматолог"}
            };
            foreach (Doctor d in doctors)
            {
                context.Doctors.Add(d);
            }

            var services = new Service[]
            {
                new Service{Name = "Прием ревматолога (первичный)", Price = 0},
                new Service{Name = "Прием ревматолога (вторичный)", Price = 600}
            };
            foreach (Service s in services)
            {
                context.Services.Add(s);
            }

            context.SaveChanges();

            var doctorProvideServices = new DoctorProvideService[]
            {
                new DoctorProvideService{DoctorID = 2, ServiceID = 1},
                new DoctorProvideService{DoctorID = 2, ServiceID = 2}
            };
            foreach (DoctorProvideService dps in doctorProvideServices)
            {
                context.ProvideServices.Add(dps);
            }

            var dateOfAppointment = new DateOfAppointment[]
            {
                new DateOfAppointment{DoctorID = 2, Date = DateTime.Today, PeriodOfWorking = "12-17"},
                new DateOfAppointment{DoctorID = 2, Date = DateTime.Today.AddDays(1), PeriodOfWorking = "8-16"},
                new DateOfAppointment{DoctorID = 2, Date = DateTime.Today.AddDays(3), PeriodOfWorking = "12-17"}
            };
            foreach (DateOfAppointment doa in dateOfAppointment)
            {
                context.AppointmentDates.Add(doa);
            }

            context.SaveChanges();
        }
    }
}
