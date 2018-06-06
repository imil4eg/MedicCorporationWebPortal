using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class AppointmentDateViewModel
    {
        public int AppointmentDateId { get; set; }
        public int DoctorId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Дата не может быть пустой")]
        [DataType(DataType.Date)]
        [DisplayName("Дата работы")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [DisplayName("Период работы")]
        public string PeriodOfWorking { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле начало не может быть пустым")]
        public int StartOfWork { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле конец не может быть пустым")]
        public int EndOfWork { get; set; }
    }
}
