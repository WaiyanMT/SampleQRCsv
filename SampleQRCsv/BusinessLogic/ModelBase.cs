using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleQRCsv.BusinessLogic
{
    public class ModelBase
    {
        public string RequestGet(string url, string path, string filters, string bearerToken)
        {
            try
            {
                HttpWebRequest httpWebRequest = HttpWebRequest.CreateHttp(url + path + "?" + filters);
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Headers.Add("Authorization", "Bearer " + bearerToken);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";

                WebResponse response = httpWebRequest.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    response.Close();

                    return responseFromServer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public string RequestPost(string url, string path, dynamic json_values, string bearerToken)
        {
            try
            {
                HttpWebRequest httpWebRequest = HttpWebRequest.CreateHttp(url + path);
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + bearerToken);
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string inputJson = JsonConvert.SerializeObject(json_values);
                    streamWriter.Write(inputJson);
                }

                WebResponse response = httpWebRequest.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    response.Close();

                    return responseFromServer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public string RequestPut(string url, string path, string filters, dynamic json_values, string bearerToken)
        {
            try
            {
                string full_path = String.IsNullOrEmpty(filters) ? url + path : url + path + "?" + filters;

                HttpWebRequest httpWebRequest = HttpWebRequest.CreateHttp(full_path);
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                httpWebRequest.PreAuthenticate = true;
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + bearerToken);
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string inputJson = JsonConvert.SerializeObject(json_values);
                    streamWriter.Write(inputJson);
                }

                WebResponse response = httpWebRequest.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    response.Close();

                    return responseFromServer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
