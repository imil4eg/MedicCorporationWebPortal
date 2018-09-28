using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MedicalCorporateWebPortal.Models
{
    public class ChangeRoleViewModel
    {
        public Guid UserID { get; set; }
        public string UserEmail { get; set; }
        public List<ApplicationRole> AllRoles { get; set; }
        public string UserRole { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<ApplicationRole>();
        }
    }
}
