﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class Doctor
    {
        [Key]
        public int ID { get; set; }
        
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        //public int? DateOfAppointmentID { get; set; }
        //[ForeignKey("DateOfAppointmentID")]
        //public virtual DateOfAppointment DateOfAppointment { get; set; }
        //
        //public int? DoctorProvideServiceID { get; set; }
        //[ForeignKey("DoctorProvideServiceID")]
        //public virtual DoctorProvideService DoctorProvideService { get; set; }

        public int SpecialtyID { get; set; }
        [ForeignKey("SpecialtyID")]
        public Specialty Specialty { get; set; }

        /// <summary>
        /// Indicates if doctor deleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}