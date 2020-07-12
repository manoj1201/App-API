using BotAPI.Entities;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BotAPI.Model
{
    public class DocumentInfo
    {
        internal List<DocumentData> GetDocumentData(string query)
        {
            string siteUrl = "https://mittal1201.sharepoint.com/";
            string userName = "manoj@mittal1201.onmicrosoft.com";
            string password = "magnet!1";

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            try
            {
                using (var clientContext = authManager.GetSharePointOnlineAuthenticatedContextTenant(siteUrl, userName, password))
                {
                    List<DocumentData> ResultantData = new List<DocumentData>();
                    string withoutExtn = Path.ChangeExtension(query, null);
                    string queryextn = Path.GetExtension(query);

                    ClientResult<ResultTableCollection> results = ProcessQuery(clientContext, query);
                    foreach (var row in results.Value[0].ResultRows)
                    {
                        string fileName = Path.GetFileName(row["Path"].ToString());
                        string NameWithoutExtn = Path.ChangeExtension(fileName, null);
                        string fileExtn = Path.GetExtension(row["Path"].ToString());
                        
                        if (queryextn == String.Empty)
                        {
                            if (query.ToLower().Contains(NameWithoutExtn.ToLower()) || NameWithoutExtn.ToLower().Contains(query.ToLower()) || withoutExtn.ToLower().Contains(NameWithoutExtn.ToLower()) || NameWithoutExtn.ToLower().Contains(withoutExtn.ToLower()))
                            {
                                string extn = Path.GetExtension(row["Path"].ToString());
                                DocumentData data = new DocumentData()
                                {
                                    Title = row["Title"].ToString(),
                                    Author = row["Author"].ToString(),
                                    DocumentPath = row["Path"].ToString(),
                                    Summary = row["HitHighlightedSummary"] != null ? row["HitHighlightedSummary"].ToString() : ""
                                };
                                ResultantData.Add(data);
                            }
                        }
                        else
                        {
                            if ((query.ToLower().Contains(NameWithoutExtn.ToLower()) || NameWithoutExtn.ToLower().Contains(query.ToLower()) || withoutExtn.ToLower().Contains(NameWithoutExtn.ToLower()) || NameWithoutExtn.ToLower().Contains(withoutExtn.ToLower())) && fileExtn.ToLower().Equals(queryextn.ToLower()))
                            {
                                string extn = Path.GetExtension(row["Path"].ToString());
                                DocumentData data = new DocumentData()
                                {
                                    Title = row["Title"].ToString(),
                                    Author = row["Author"].ToString(),
                                    DocumentPath = row["Path"].ToString(),
                                    Summary = row["HitHighlightedSummary"] != null ? row["HitHighlightedSummary"].ToString() : ""
                                };
                                ResultantData.Add(data);
                            }
                        }
                    }
                    return ResultantData;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ClientResult<ResultTableCollection> ProcessQuery(ClientContext clientContext, string query)
        {
            try
            {
                KeywordQuery keywordQuery = new KeywordQuery(clientContext);
                keywordQuery.QueryText = query;
                keywordQuery.RowLimit = 150;
                keywordQuery.StartRow = 0;
                keywordQuery.RefinementFilters.Add("(fileExtension:or(\"txt\",\"docx\",\"pdf\",\"doc\",\"xls\",\"xlsx\",\"xlsm\",\"ppt\",\"pptx\",\"mpp\",\"csv\"))");//\\" ???? 68746d6c\\"
                SearchExecutor searchExec = new SearchExecutor(clientContext);
                ClientResult<ResultTableCollection> results = searchExec.ExecuteQuery(keywordQuery);
                clientContext.ExecuteQuery();
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}