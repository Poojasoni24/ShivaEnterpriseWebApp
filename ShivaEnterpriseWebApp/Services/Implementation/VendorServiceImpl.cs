using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;


    namespace ShivaEnterpriseWebApp.Services.Implementation
    {
        public class VendorServiceImpl : IVendorServiceImpl
        {
            private readonly JObject urlCollections;

            public VendorServiceImpl()
            {
                urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
            }
            public async Task<(bool success, string message)> AddVendorDetailsAsync(Vendor vendor, string authToken)
            {
                try
                {
                    //Create json string to prepare input for api                
                    string json = JsonConvert.SerializeObject(vendor);

                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = urlCollections["baseUrl"].ToString() + urlCollections["addvendorUrl"].ToString();
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);
                    request.Content = data;

                    //Pass in the full URL and the json string content
                    var response = await client.SendAsync(request);

                    //It would be better to make sure this request actually made it through
                    var result = await response.Content.ReadAsStringAsync();

                    //close out the client
                    client.Dispose();

                    if (response.StatusCode != HttpStatusCode.OK)
                        return (false, result);

                    return (true, result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<(bool successs, string message)> DeleteVendor(string vendorId, string authToken)
            {
                var url = urlCollections["baseUrl"].ToString() + urlCollections["deletevendorUrl"] + "?vendorId=" + vendorId;
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

                //Pass in the full URL and the json string content
                var response = await client.SendAsync(request);

                //It would be better to make sure this request actually made it through
                var result = await response.Content.ReadAsStringAsync();

                //close out the client
                client.Dispose();

                if (response.StatusCode != HttpStatusCode.OK)
                    return (false, result);

                return (true, result);
            }

            public async Task<(bool success, string message)> EditVendorDetailsAsync(Vendor vendor, string authToken)
            {
                try
                {
                    //Create json string to prepare input for api                
                    string json = JsonConvert.SerializeObject(vendor);

                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = urlCollections["baseUrl"].ToString() + urlCollections["editvendorUrl"] + "?id=" + vendor.VendorId.ToString();
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(new HttpMethod("PUT"), url);
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);
                    request.Content = data;

                    //Pass in the full URL and the json string content
                    var response = await client.SendAsync(request);

                    //It would be better to make sure this request actually made it through
                    var result = await response.Content.ReadAsStringAsync();

                    //close out the client
                    client.Dispose();

                    if (response.StatusCode != HttpStatusCode.NoContent)
                        return (false, result);

                    return (true, result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<Vendor> GetVendorById(string vendorId, string authToken)
            {
                var url = urlCollections["baseUrl"].ToString() + urlCollections["getvendorbyidUrl"] + "?vendorId=" + vendorId;
                var client = new HttpClient();

                var request = new HttpRequestMessage(new HttpMethod("GET"), url);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


                //Pass in the full URL and the json string content
                var response = await client.SendAsync(request);

                //It would be better to make sure this request actually made it through
                var result = await response.Content.ReadAsStringAsync();

                //close out the client
                client.Dispose();

                var vendorDetail = JsonConvert.DeserializeObject<Vendor>(result);

                return vendorDetail;
            }

            public async Task<List<Vendor>> GetVendorList(string authToken)
            {
                var url = urlCollections["baseUrl"].ToString() + urlCollections["getallvendorUrl"].ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("GET"), url);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

                //Pass in the full URL and the json string content
                var response = await client.SendAsync(request);

                //It would be better to make sure this request actually made it through
                var result = await response.Content.ReadAsStringAsync();

                //close out the client
                client.Dispose();

                var vendorDetail = JsonConvert.DeserializeObject<List<Vendor>>(result);

                return vendorDetail;
            }
        }
    }
