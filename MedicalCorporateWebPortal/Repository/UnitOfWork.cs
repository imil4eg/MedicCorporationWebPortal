using MedicalCorporateWebPortal.AppData;

namespace MedicalCorporateWebPortal.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MedicCroporateContext _context;

        public UnitOfWork(MedicCroporateContext context)
        {
            _context = context;
            Appointments = new AppointmentRepository(_context);
            Patients = new PatientRepository(_context);
            Doctors = new DoctorRepository(_context);
            Employees = new EmployeeRepository(_context);
            Users = new UserRepository(_context);
            DatesOfAppointments = new DateOfAppointmentRepository(_context);
            DoctorProvideServices = new DoctorProvideServiceRepository(_context);
            ReservedTimes = new ReservedTimeRepository(_context);
            Services = new ServiceRepository(_context);
            Specialtys = new SpecialtyRepository(_context);
        }

        public IAppointmentRepository Appointments { get; private set; }
        public IPatientRepository Patients { get; private set; }
        public IDoctorRepository Doctors { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IUserRepository Users { get; private set; }
        public IDateOfAppointmentRepository DatesOfAppointments { get; private set; }
        public IDoctorProvideServiceRepository DoctorProvideServices { get; private set; }
        public IReservedTimeRepository ReservedTimes { get; private set; }
        public IServiceRepository Services { get; private set; }
        public ISpecialtyRepository Specialtys { get; private set; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
