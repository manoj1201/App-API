using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SP = Microsoft.SharePoint.Client;

namespace BotAPI
{
    public class PatientData
    {
        static string siteUrl = "https://mittal1201.sharepoint.com/sites/CommSiteHub/";
        static string userName = "manoj@mittal1201.onmicrosoft.com";
        static string password = "Magnet!1";
        static string listName = "Patients";
        internal List<PatientDto> SearchPatient(string name)
        {
            List<PatientDto> list = new List<PatientDto>();

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            try
            {
                using (var clientContext = authManager.GetSharePointOnlineAuthenticatedContextTenant(siteUrl, userName, password))
                {
                    SP.List oList = clientContext.Web.Lists.GetByTitle(listName);
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = string.Format(
                              "<View>" +
                                  "<Query>" +
                                      "<Where>" +
                                        "<Or>" +
                                            
                                         
                                          "<Contains>" +
                                              "<FieldRef Name='PatientName'/>" +
                                              "<Value Type='Text'>{0}</Value>" +
                                          "</Contains>" +
                                         
                                           "<Eq>" +
                                              "<FieldRef Name='PatientGender'/>" +
                                              "<Value Type='Text'>{0}</Value>" +
                                          "</Eq>" +
                                        "</Or>" +
                                      "</Where>" +
                                  "</Query>" +
                                  "<RowLimit>{1}</RowLimit>" +
                              "</View>",
                              name, 10);

                    ListItemCollection collListItem = oList.GetItems(camlQuery);

                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {
                        var items = oListItem.FieldValues;
                        PatientDto card = new PatientDto();
                        foreach (var item in items)
                        {
                            card.Id = item.Key.Equals("ID") ? item.Value.ToString() : card.Id;
                            card.PatientName = item.Key.Equals("PatientName") ? item.Value.ToString() : card.PatientName;
                            card.PatientAge = item.Key.Equals("PatientAge") ? item.Value.ToString() : card.PatientAge;
                            card.PatientContactAddress = item.Key.Equals("PatientContactAddress") ? item.Value.ToString() : card.PatientContactAddress;
                            card.PatientContactNo = item.Key.Equals("PatientContactNo") ? item.Value.ToString() : card.PatientContactNo;
                            card.PatientGender = item.Key.Equals("PatientGender") ? item.Value.ToString() : card.PatientGender;
                            if (item.Key.Equals("Photograph"))
                            {
                                dynamic pic = item.Value;
                                card.Photograph = pic.Url.ToString();
                            }
                            card.PatientMedicalHistory = item.Key.Equals("PatientMedicalHistory") ? item.Value.ToString() : card.PatientMedicalHistory;
                        }
                        list.Add(card);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}