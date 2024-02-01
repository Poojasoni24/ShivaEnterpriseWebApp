using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class TaxServiceImpl:ITaxServiceImpl
    {
        private readonly JObject urlCollections;
        public TaxServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<(bool success, string message)> AddTaxDetailsAsync(Tax Tax, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(Tax);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addTaxUrl"].ToString();
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

        public async Task<(bool successs, string message)> DeleteTax(string TaxId, string authToken)
        {
            string json = "{ \"TaxId\": \"" + TaxId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deleteTaxUrl"] + "?TaxId=" + TaxId;
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

        public async Task<(bool success, string message)> EditTaxDetailsAsync(Tax Tax, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(Tax);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editTaxUrl"] + "?id=" + Tax.TaxId.ToString();
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

        public async Task<Tax> GetTaxById(string TaxId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getTaxbyidUrl"] + "?TaxId=" + TaxId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var TaxDetail = JsonConvert.DeserializeObject<Tax>(result);

            return TaxDetail;
        }

        public async Task<List<Tax>> GetTaxList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallTaxUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var TaxDetail = JsonConvert.DeserializeObject<List<Tax>>(result);

            return TaxDetail;
        }
    }
}
