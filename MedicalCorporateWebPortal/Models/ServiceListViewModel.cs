using MedicalCorporateWebPortal.AppData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MedicalCorporateWebPortal.Models
{
    public class ServiceListViewModel
    {
        public List<Service> Services;
        public IList<SelectListItem> ServiceItems { get; set; }
        public int SelectedServiceID { get; set; }
    }
}
