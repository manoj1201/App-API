using BotAPI.Entities;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client.UserProfiles;

namespace BotAPI.Model
{
    public class ProfileInformation
    {
        public List<Profile> GetProfileData(string query)
        {
            string siteUrl = "https://mittal1201.sharepoint.com/";
            string userName = "manoj@mittal1201.onmicrosoft.com";
            string password = "magnet!1";

            List<Profile> profiles = new List<Profile>();
            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            try
            {
                using (var clientContext = authManager.GetSharePointOnlineAuthenticatedContextTenant(siteUrl, userName, password))
                {
                    PeopleManager peopleManager = new PeopleManager(clientContext);

                    //Fetching search result
                    ClientResult<ResultTableCollection> results = ProcessQuery(clientContext, query);
                    if (results.Value[0].RowCount > 0)
                    {
                        foreach (var row in results.Value[0].ResultRows)
                        {
                            string accountInfo = Convert.ToString(row["AccountName"]);
                            Profile profile = new Profile();
                            PersonProperties personProperties = peopleManager.GetPropertiesFor(accountInfo);
                            clientContext.Load(personProperties);
                            clientContext.ExecuteQuery();
                            profile.PersonBirthday = Convert.ToString(personProperties.UserProfileProperties["SPS-Birthday"]);
                            profile.PersonName = Convert.ToString(personProperties.UserProfileProperties["PreferredName"]);
                            profile.PersonDepartment = Convert.ToString(personProperties.UserProfileProperties["Department"]);
                            profile.PersonImgurl = Convert.ToString(personProperties.UserProfileProperties["PictureURL"]);
                            profile.PersonEmail = Convert.ToString(personProperties.UserProfileProperties["WorkEmail"]);
                            profile.PersonJobtitle = Convert.ToString(personProperties.UserProfileProperties["SPS-JobTitle"]);
                            profile.PersonWorkphno = Convert.ToString(personProperties.UserProfileProperties["WorkPhone"]);
                            profile.PersonCellPhno = Convert.ToString(personProperties.UserProfileProperties["CellPhone"]);
                            profiles.Add(profile);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return profiles;
        }
        private static ClientResult<ResultTableCollection> ProcessQuery(ClientContext ctx, string keywordQueryValue)
        {
            try
            {
                var queryID = "b09a7990-05ea-4af9-81ef-edfab16c4e31";
                Guid guid = new Guid(queryID);
                KeywordQuery keywordQuery = new KeywordQuery(ctx);
                keywordQuery.QueryText = keywordQueryValue;
                keywordQuery.RowLimit = 500;
                keywordQuery.StartRow = 0;
                keywordQuery.SourceId = guid;
                SearchExecutor searchExec = new SearchExecutor(ctx);
                ClientResult<ResultTableCollection> results = searchExec.ExecuteQuery(keywordQuery);
                ctx.ExecuteQuery();
                return results;
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}