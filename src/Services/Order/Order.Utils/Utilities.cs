using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Order.Domain;

namespace Order.Utils
{
    public class Utilities
    {
        public static string urlAutenticate = "http://localhost:18839/identity/api/v1/users/auth";
        public static bool Auth(string token)
        {
            string apiUrl = urlAutenticate;

            string authToken = token;

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                try
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                        request.Method = "GET";
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string responseText = reader.ReadToEnd();
                                    AuthInfo responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthInfo>(responseText);
                                    return (bool)responseData.status;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
                catch (HttpRequestException ex)
                {
                    throw new Exception("Error en conexion al api IDENTITY.API");
                }

            }
        }

        public static AuthInfo AuthInfoGet(string token)
        {
            string apiUrl = urlAutenticate;

            string authToken = token;

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                try
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                        request.Method = "GET";
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string responseText = reader.ReadToEnd();
                                    AuthInfo responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthInfo>(responseText);
                                    return responseData;
                                }
                            }
                        }
                        else
                        {
                            return new AuthInfo
                            {
                                status = false,
                                username = "",
                                role = ""
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new AuthInfo
                        {
                            status = false,
                            username = "",
                            role = ""
                        };
                    }

                }
                catch (HttpRequestException ex)
                {
                    throw new Exception("Error en conexion al api IDENTITY.API");
                }

            }
        }

    }
}
