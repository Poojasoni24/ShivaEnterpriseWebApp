using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class ProductTypeServiceImpl : IProductTypeServiceImpl
    {
        private readonly JObject urlCollections;

        public ProductTypeServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<(bool success, string message)> AddProductTypeDetailsAsync(ProductType productType, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(productType);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addproducttypeUrl"].ToString();
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

        public async Task<(bool successs, string message)> DeleteProductType(string productTypeId, string authToken)
        {
            string json = "{ \"productTypeId\": \"" + productTypeId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deleteproducttypeUrl"] + "?productTypeId=" + productTypeId;
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

        public async Task<(bool success, string message)> EditProductTypeDetailsAsync(ProductType productType, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(productType);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editproducttypeUrl"] + "?id=" + productType.ProductTypeId.ToString();
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

        public async Task<ProductType> GetProductTypeById(string productTypeId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getproducttypebyidUrl"] + "?productTypeId=" + productTypeId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var productTypeDetail = JsonConvert.DeserializeObject<ProductType>(result);

            return productTypeDetail;
        }

        public async Task<List<ProductType>> GetProductTypeList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallproducttypeUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var productTypeDetail = JsonConvert.DeserializeObject<List<ProductType>>(result);

            return productTypeDetail;
        }
    }
}
