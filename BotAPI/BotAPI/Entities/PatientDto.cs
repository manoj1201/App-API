using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotAPI
{
    public class PatientDto
    {
        public string Id { get; set; }
        public string PatientName { get; set; }
        public string PatientAge { get; set; }
        public string PatientGender { get; set; }
        public string PatientContactNo { get; set; }
        public string PatientContactAddress { get; set; }
        public string PatientMedicalHistory { get; set; }
        public string Photograph { get; set; }
      
    }
}