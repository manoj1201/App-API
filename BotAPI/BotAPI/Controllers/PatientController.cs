using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BotAPI
{
    public class PatientController : ApiController
    {
        [Route("api/Patient")]
        [HttpGet]
        public List<PatientDto> Patient(string name)
        {
            PatientData patientdata = new PatientData();
            List<PatientDto> list = patientdata.SearchPatient(name);
            return list;
        }
    }
}
