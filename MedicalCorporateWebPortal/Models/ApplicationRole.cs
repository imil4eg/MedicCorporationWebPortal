using Microsoft.AspNetCore.Identity;
using System;

namespace MedicalCorporateWebPortal.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();
        }

        public ApplicationRole(string roleName) : this()
        {
            Name = roleName;
        }

        public ApplicationRole(string name, string id)
        {
            Name = name;
            Id = Guid.Parse(id);
        }
    }
}
