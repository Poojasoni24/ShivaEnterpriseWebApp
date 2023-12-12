using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using ShivaEnterpriseWebApp.Model;
using System.ComponentModel.Design;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class CompanyServiceImpl : ICompany
    {
        private readonly JObject urlCollections;
        public CompanyServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<List<CompanyDTO>> GetCompanyDetailsAsync()
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["GetCompaniesUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            //request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + AuthToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var companies = JsonConvert.DeserializeObject<List<CompanyDTO>>(result);

            return companies;
        }

        public async Task<CompanyDTO> GetCompanyDetailById(Guid companyId)
        {
            string json = "{ \"companyId\": \"" + companyId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["GetCompanyByIdUrl"] +"?CompanyId="+companyId.ToString();
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            //request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + AuthToken);
            //request.Content = new StringContent(data);
                

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var companies = JsonConvert.DeserializeObject<CompanyDTO>(result);

            return companies;
        }

        public async Task<(bool successs,string message)> DeleteCompany(Guid companyId)
        {
            string json = "{ \"CompanyId\": \"" + companyId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["DeleteCompanyUrl"] + "?CompanyId=" + companyId.ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            //request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + AuthToken);
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

        public async Task<(bool success, string message)> AddCompanyDetailsAsync(CompanyDTO company)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(company);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["AddCompaniesUrl"].ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                //request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + AuthToken);
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

        public async Task<(bool success, string message)> EditCompanyDetailsAsync(Guid id,CompanyDTO company)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(company);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["EditCompaniesUrl"] + "?id=" + id.ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("PUT"), url);
                //request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + AuthToken);
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
