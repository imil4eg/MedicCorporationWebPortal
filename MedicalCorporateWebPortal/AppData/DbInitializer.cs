using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.AppData
{
    public static class DbInitializer
    {
        public async static Task Initialize(MedicCroporateContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            string[] roleNames = { "Администратор", "Пациент", "Врач", "Бухгалтер", "Ресепшен" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            var powerUser = new ApplicationUser
            {
                UserName = "Admin",
                Password = "Admin",
                Email = "Admin@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Admin",
                FirstName = "Admin"
            };

            var userPatient = new ApplicationUser
            {
                UserName = "Patient",
                Password = "Patient",
                Email = "Patient@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Patient",
                FirstName = "Patient",
                Role = UserRole.Пациент
            };

            var userDoctor = new ApplicationUser
            {
                UserName = "Doctor",
                Password = "Doctor",
                Email = "Doctor@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Doctor",
                FirstName = "Doctor",
                Role = UserRole.Врач
            };

            var userCalculator = new ApplicationUser
            {
                UserName = "Calcuc",
                Password = "Calcuc",
                Email = "Calcuc@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Calcuc",
                FirstName = "Calcuc",
                Role = UserRole.Бухгалтер
            };


            var _user = await userManager.FindByEmailAsync(powerUser.Email);
            if (_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, powerUser.Password);
                if (createPowerUser.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(powerUser.UserName);
                    var employee = new Employee
                    {
                        UserID = user.Id,
                        ApplicationUser = user
                    };

                    await context.Employees.AddAsync(employee);
                    await userManager.AddToRoleAsync(powerUser, "Администратор");
                }
            }

            _user = await userManager.FindByEmailAsync(userPatient.Email);
            if (_user == null)
            {
                var createUserPatient = await userManager.CreateAsync(userPatient, userPatient.Password);
                if (createUserPatient.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(userPatient.UserName);
                    var patient = new Patient
                    {
                        UserID = user.Id,
                        ApplicationUser = user
                    };

                    await context.Patients.AddAsync(patient);
                    await userManager.AddToRoleAsync(userPatient, "Пациент");
                }
            }

            _user = await userManager.FindByEmailAsync(userCalculator.Email);
            if (_user == null)
            {
                var createUserCalcuc = await userManager.CreateAsync(userCalculator, userCalculator.Password);
                if (createUserCalcuc.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(userCalculator.UserName);
                    var employee = new Employee
                    {
                        ApplicationUser = user,
                        UserID = user.Id
                    };

                    await context.Employees.AddAsync(employee);
                    await userManager.AddToRoleAsync(userCalculator, "Бухгалтер");
                }
            }

            var specialtities = new[]
            {
                new Specialty{Name = "Нет специальности"},
                new Specialty{Name = "Хирург"},
                new Specialty{Name = "Косметолог"},
                new Specialty{Name = "Физиотерапевт"},
                new Specialty{Name = "Ревматолог"}
            };
            foreach (Specialty s in specialtities)
            {
                context.Add(s);
            }

            context.SaveChanges();

            var users = new ApplicationUser[]
            {
                new ApplicationUser{Id = Guid.NewGuid(), Email = "pupkin@mail.ru", UserName = "qwerty", Password = "qwerty", LastName = "Пупкин", FirstName = "Вася", MiddleName = "Васильевич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new ApplicationUser{Id = Guid.NewGuid(), Email = "Vasiliev@mail.ru", UserName = "qwerty1", Password = "qwerty1", LastName = "Васильев", FirstName = "Вася", MiddleName = "Васильевич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new ApplicationUser{Id = Guid.NewGuid(), Email = "Ilin@mail.ru", UserName = "qwerty2", Password = "qwerty2", LastName = "Ильин", FirstName = "Илья", MiddleName = "Ильич", Gender = Gender.Мужской, Role = UserRole.Врач},
                new ApplicationUser{Id = Guid.NewGuid(), Email = "Petyaev@mail.ru", UserName = "qwerty3", Password = "qwerty3", LastName = "Петяев", FirstName = "Петя", MiddleName = "Петькович", Gender = Gender.Мужской, Role = UserRole.Врач}
            };
            foreach (ApplicationUser u in users)
            {
                context.Users.Add(u);
                await userManager.CreateAsync(u, u.Password);
                await userManager.AddToRoleAsync(u, UserRole.Врач.ToString());
            }

            context.SaveChanges();

            for (int i = 1; i < users.Length + 1; i++)
            {
                context.Employees.Add(new Employee { UserID = users[i - 1].Id });
            }

            context.SaveChanges();

            var doctors = new Doctor[]
            {
                new Doctor{EmployeeID = 3, SpecialtyID = context.Specialties.FirstOrDefault(s => s.Name == "Хирург").ID},
                new Doctor{EmployeeID = 4, SpecialtyID = context.Specialties.FirstOrDefault(s => s.Name == "Косметолог").ID},
                new Doctor{EmployeeID = 5, SpecialtyID = context.Specialties.FirstOrDefault(s => s.Name == "Физиотерапевт").ID },
                new Doctor{EmployeeID = 6, SpecialtyID = context.Specialties.FirstOrDefault(s => s.Name == "Ревматолог").ID}
            };
            foreach (Doctor d in doctors)
            {
                context.Doctors.Add(d);
            }

            var services = new Service[]
            {
                new Service{Name = "Прием ревматолога (первичный)", Price = 0, Description = "Прием", IsDeleted = false },
                new Service{Name = "Прием ревматолога (вторичный)", Price = 600, Description = "Прием 2", IsDeleted = false}
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
