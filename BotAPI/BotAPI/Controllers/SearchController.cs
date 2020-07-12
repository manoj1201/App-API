using BotAPI.Entities;
using BotAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BotAPI.Controllers
{
    public class SearchController : ApiController
    {
        [Route("api/profile")]
        [HttpGet]
        public List<Profile> GetPeopleData(string query)
        {
            try
            {
                ProfileInformation profile = new ProfileInformation();
                
                return profile.GetProfileData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("api/Document")]
        [HttpGet]
        public List<DocumentData> GetDocumentData(string query)
        {
            try
            {
                DocumentInfo doc = new DocumentInfo();
                return doc.GetDocumentData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
