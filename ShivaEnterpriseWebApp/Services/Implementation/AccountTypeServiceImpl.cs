using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class AccountTypeServiceImpl : IAccountTypeServiceImpl
    {
        private readonly JObject urlCollections;

        public AccountTypeServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }
        public async Task<(bool success, string message)> AddAccountTypeDetailsAsync(AccountType accountType, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(accountType);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addaccounttypeUrl"].ToString();
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

        public async Task<(bool successs, string message)> DeleteAccountType(string accountTypeId, string authToken)
        {
            string json = "{ \"accountTypeId\": \"" + accountTypeId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deleteaccounttypeUrl"] + "?accountTypeId=" + accountTypeId;
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

        public async Task<(bool success, string message)> EditAccountTypeDetailsAsync(AccountType accountType, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(accountType);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editaccounttypeUrl"] + "?id=" + accountType.AccountTypeId.ToString();
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

        public async Task<AccountType> GetAccountTypeById(string accountTypeId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getaccounttypebyidUrl"] + "?accountTypeId=" + accountTypeId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var accountTypeDetailId = JsonConvert.DeserializeObject<AccountType>(result);

            return accountTypeDetailId;
        }

        public async Task<List<AccountType>> GetAccountTypeList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallaccounttypeUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var getaccountTypeDetails = JsonConvert.DeserializeObject<List<AccountType>>(result);

            return getaccountTypeDetails;
        }
    }
}
