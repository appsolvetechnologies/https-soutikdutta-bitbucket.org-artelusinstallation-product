using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Artelus.Model
{
    public static class RestCalls
    {
        readonly static string baseUrl = "http://hsnet.artelus.com";
        //readonly static string baseUrl = "http://drds.artelus.com";
        //readonly static string baseUrl = "http://192.168.56.101";

        readonly static string postUrl = "/diabetic_retinopathy/predictions";
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        static int MaxAttempts = 30;
        static string result;
        static Dictionary<string, string> post_dict, get_dict;

        public static string RestPredict(string loc, string prediction_type)
        {
            result = GetTaskUrl(loc, prediction_type);
            Console.WriteLine(result);
            post_dict = serializer.Deserialize<Dictionary<string, string>>(result);
            for (var i = 0; i < MaxAttempts; i++)
            {
                Thread.Sleep(3000);
                result = GetPredictions(post_dict["taskstatus"]);
                Console.WriteLine(result);
                get_dict = serializer.Deserialize<Dictionary<string, string>>(result);
                if (get_dict["state"] == "SUCCESS") { break; }
            }
            return result;
        }

        public static string GetTaskUrl(string fileName, string prediction_type)
        {
            NameValueCollection files = new NameValueCollection();
            files.Add("image", fileName);
            //files.Add("prediction_type", "hansa");
            string response = sendHttpRequest(baseUrl + postUrl, prediction_type, files);
            return response;
        }

        public static string GetPredictions(string taskUrl)
        {
            string response = getResponse(baseUrl + taskUrl);
            return response;
        }

        private static string sendHttpRequest(string url, string prediction_type, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
            byte[] boundaryBytesF = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            // Create the request and set parameters
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;


            // Get request stream
            Stream requestStream = request.GetRequestStream();


            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            byte[] predictionItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", "prediction_type", prediction_type));
            requestStream.Write(predictionItemBytes, 0, predictionItemBytes.Length);


            if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (File.Exists(files[key]))
                    {
                        int bytesRead = 0;
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            fileStream.Close();
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadToEnd();
            };
        }

        private static string getResponse(string finalUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);
            request.Method = "GET";
            string responseJson = string.Empty;
            WebResponse webResponse = request.GetResponse();

            Stream webStream = null;
            try
            {
                webStream = webResponse.GetResponseStream();
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        responseJson = responseReader.ReadToEnd();
                    }
                    webStream = null;
                }
            }
            finally
            {
                if (webStream != null)
                    webStream.Dispose();
            }

            return responseJson;
        }

        public static APIResult SyncReport(string url, string data, bool isUpdate)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = isUpdate == false ? "POST" : "PATCH";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            APIResult val = new JavaScriptSerializer().Deserialize<APIResult>(result);
            return val;
        }
    }
}
