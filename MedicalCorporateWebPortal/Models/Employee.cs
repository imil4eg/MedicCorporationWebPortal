﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public Guid UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}