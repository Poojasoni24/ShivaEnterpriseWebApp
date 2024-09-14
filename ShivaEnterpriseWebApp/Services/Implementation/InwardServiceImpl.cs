using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;
using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class InwardServiceImpl : IInwardServiceImpl
    {
        private readonly JObject urlCollections;

        public InwardServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }

        public async Task<(bool success, string message)> AddInwardDetailsAsync(Inward inward, string authToken)
        {
            try
            {
                string json = JsonConvert.SerializeObject(inward);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addInwardUrl"].ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);
                request.Content = data;

                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
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

        public async Task<(bool success, string message)> DeleteInward(Guid inwardId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deleteInwardUrl"] + "?inwardsId=" + inwardId;
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            client.Dispose();

            if (response.StatusCode != HttpStatusCode.OK)
                return (false, result);

            return (true, result);
        }

        public async Task<(bool success, string message)> EditInwardDetailsAsync(Inward inward, string authToken)
        {
            try
            {
                string json = JsonConvert.SerializeObject(inward);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editInwardUrl"] + "?id=" + inward.InwardId.ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("PUT"), url);
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);
                request.Content = data;

                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
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

        public async Task<Inward> GetInwardById(Guid inwardId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getInwardByIdUrl"] + "?inwardId=" + inwardId;
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            client.Dispose();

            var inwardDetail = JsonConvert.DeserializeObject<Inward>(result);
            return inwardDetail;
        }

        public async Task<List<Inward>> GetInwardList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getAllInwardsUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            client.Dispose();

            var inwardDetail = JsonConvert.DeserializeObject<List<Inward>>(result);
            return inwardDetail;
        }

        public async Task<Product> GetProductByPurchseOrderId(Guid productId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getproductbyidUrl"] + "?productId=" + productId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();
            var productDetail = JsonConvert.DeserializeObject<Product>(result);
            //var productDetail = JsonConvert.DeserializeObject<Product>(result);

            return productDetail;
        }
    }
}
