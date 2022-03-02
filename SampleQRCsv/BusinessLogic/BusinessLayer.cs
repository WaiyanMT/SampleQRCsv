using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleQRCsv.BusinessLogic
{
    public class BusinessLayer : ModelBase
    {
        string serverUrl = ConfigurationManager.AppSettings["APIUrl"];

        public string GetToken()
        {
            string bearerToken = "";

            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            try
            {
                dynamic json_request = new ExpandoObject();
                json_request.username = username;
                json_request.password = password;

                string responseFromServer = RequestPost(serverUrl, "Authenticate/Login", json_request, bearerToken);

                if (responseFromServer != "")
                {
                    JObject result = JObject.Parse(responseFromServer);

                    bearerToken = result["token"].ToString();

                    Log.Information($"Successfully get the token for user name {username} and password {password} ");
                }
                else
                {
                    Log.Information($"Not successful get the token for {username} and password {password} ");
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }

            return bearerToken;
        }

        public dynamic RetrieveResource(string machineid, string bearerToken)
        {

            // Retrieve resourceid from Resource table
            dynamic json_request = new ExpandoObject();

            try
            {
                string filter = "tagid=" + machineid;
                string responseFromServer = RequestGet(serverUrl, "Resource/getresourcebytagid", filter, bearerToken);

                if (responseFromServer != "")
                {
                    JObject result = JObject.Parse(responseFromServer);

                    json_request.Id = Convert.ToInt32(JObject.Parse(responseFromServer)["id"].ToString());

                    Log.Information($"Successfully retrieved resourceid {responseFromServer}");
                }
                else
                {
                    json_request.Id = 0;
                    Log.Information($"retrieved itemtagid is zero");
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }

            return json_request;
        }
    }
}
