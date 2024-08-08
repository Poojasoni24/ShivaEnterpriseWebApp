using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class StockServiceImpl : IStockServiceImpl
    {
        private readonly JObject urlCollections;

        public StockServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }

        public async Task<Stock> GetStockByProductId(Guid productId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getstockbyproductidUrl"] + "?productId=" + productId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            Stock stockDetail = JsonConvert.DeserializeObject<Stock>(result);

            return stockDetail;
        }

        public async Task<List<Stock>> GetStockList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallstockUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            List<Stock> stockList = new List<Stock>();

            try
            {
                stockList = JsonConvert.DeserializeObject<List<Stock>>(result);
            }
            catch (Exception ex)
            {
                stockList = null;
            }

            return stockList;
        }

        public async Task<List<Stock>> AddEditStockList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallstockUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            List<Stock> stockList = new List<Stock>();

            try
            {
                stockList = JsonConvert.DeserializeObject<List<Stock>>(result);
            }
            catch (Exception ex)
            {
                stockList = null;
            }

            return stockList;
        }

        public async Task<(bool success, string value)> AddEditStockDetailsAsync(List<Stock> stock, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(stock);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addeditstockdetailsUrl"].ToString();
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

    }
}
