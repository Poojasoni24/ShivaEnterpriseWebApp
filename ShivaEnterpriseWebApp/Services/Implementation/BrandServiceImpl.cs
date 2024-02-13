using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class BrandServiceImpl: IBrandServiceImpl
    {
        private readonly JObject urlCollections;
        public BrandServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<(bool success, string message)> AddBrandDetailsAsync(Brand Brand, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(Brand);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addbrandUrl"].ToString();
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

      
        public async Task<(bool successs, string message)> DeleteBrand(Guid brandId, string authToken)
        {
            string json = "{ \"brandId\": \"" + brandId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deletebrandUrl"] + "?brandId=" + brandId;
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

        public async Task<(bool success, string message)> EditBrandDetailsAsync(Brand Brand, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(Brand);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editbrandUrl"] + "?id=" + Brand.BrandId.ToString();
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

        public async Task<Brand> GetBrandById(Guid BrandId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getbrandbyidUrl"] + "?BrandId=" + BrandId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var brandDetail = JsonConvert.DeserializeObject<Brand>(result);

            return brandDetail;
        }

        public async Task<List<Brand>> GetBrandList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallbrandUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var brandDetail = JsonConvert.DeserializeObject<List<Brand>>(result);

            return brandDetail;
        }

    }
}

