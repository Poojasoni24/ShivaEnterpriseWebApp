using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using System.Net;
using System.Text;
using ShivaEnterpriseWebApp.Services.Interface;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class PurchaseOrderReturnServiceImpl:IPurchaseOrderReturnServiceImpl
    {
        private readonly JObject urlCollections;

        public PurchaseOrderReturnServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<(bool success, string message)> AddpurchaseReturnDetailsAsync(PurchaseOrderReturn purchaseReturn, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(purchaseReturn);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addPurchaseOrderReturnUrl"].ToString();
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

        public async Task<(bool successs, string message)> DeletepurchaseReturn(Guid purchaseReturnId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deletePurchaseOrderReturnUrl"] + "?purchaseReturnId=" + purchaseReturnId;
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

        public async Task<(bool success, string message)> EditpurchaseReturnDetailsAsync(PurchaseOrderReturn purchaseReturn, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(purchaseReturn);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editPurchaseOrderReturnUrl"] + "?id=" + purchaseReturn.PurchaseReturnId.ToString();
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

                if (response.StatusCode != HttpStatusCode.OK)
                    return (false, result);

                return (true, result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PurchaseOrderReturn> GetpurchaseReturnById(Guid purchaseReturnId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getPurchaseOrderReturnbyidUrl"] + "?purchaseReturnId=" + purchaseReturnId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var purchaseReturnDetail = JsonConvert.DeserializeObject<PurchaseOrderReturn>(result);

            return purchaseReturnDetail;
        }

        public async Task<List<PurchaseOrderReturn>> GetpurchaseReturnList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallPurchaseOrderReturnUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var purchaseReturnDetail = JsonConvert.DeserializeObject<List<PurchaseOrderReturn>>(result);

            return purchaseReturnDetail;
        }
    }
}
