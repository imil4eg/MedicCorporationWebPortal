using System;

namespace MedicalCorporateWebPortal.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        IEmployeeRepository Employees { get; }
        IUserRepository Users { get; }
        IDateOfAppointmentRepository DatesOfAppointments { get; }
        IDoctorProvideServiceRepository DoctorProvideServices { get; }
        IReservedTimeRepository ReservedTimes { get; }
        IServiceRepository Services { get; }
        ISpecialtyRepository Specialtys { get; }
        int Save();
    }
}
