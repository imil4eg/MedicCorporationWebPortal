using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace MedicalCorporateWebPortal.Models
{
    public class DoctorViewModel
    {
        public User User { get; set; }

        public Doctor Doctor { get; set; }

        public string SpecialtyName { get; set; }

        public IEnumerable<DoctorProvideService> ProvideServices { get; set; }

        public IEnumerable<DateOfAppointment> DatesOfAppointment { get; set; }

        public IEnumerable<ReservedTime> ReservedTimes { get; set; }

        public IEnumerable<Service> Services { get; set; }  
        public string SelectedService { get; set; }

        public DateTime Time { get; set; }
    }
}
