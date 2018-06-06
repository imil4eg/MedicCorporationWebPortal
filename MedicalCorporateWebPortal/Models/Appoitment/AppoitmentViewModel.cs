using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class AppoitmentViewModel
    {
        /// <summary>
        /// Id of existing appoitment
        /// </summary>
        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorFirstName { get; set; }

        public Guid PatientId { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }

        public int ServiceID { get; set; }
        public string ServiceName { get; set; }

        public int ReservedTimeID { get; set; }

        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Информация не может быть пустой")]
        [DataType(DataType.MultilineText)]
        public string Information { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Результат не может быть пустым")]
        [DataType(DataType.MultilineText)]
        public string Result { get; set; }
    }
}
