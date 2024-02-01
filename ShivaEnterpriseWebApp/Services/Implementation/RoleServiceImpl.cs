using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.Net;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class RoleServiceImpl : IRoleServiceImpl
    {
        private readonly JObject urlCollections;
        public RoleServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }

        public async Task<(bool success, string message)> AddRoleAsync(Role role, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(role);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["addroleUrl"].ToString();
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

        public async Task<(bool successs, string message)> DeleteRole(string roleId, string authToken)
        {
            string json = "{ \"roleId\": \"" + roleId + "\" }";
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = urlCollections["baseUrl"].ToString() + urlCollections["deleteroleUrl"] + "?roleId=" + roleId.ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), url);
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

        public async Task<(bool success, string message)> EditRoleAsync(Role role, string authToken)
        {
            try
            {
                //Create json string to prepare input for api                
                string json = JsonConvert.SerializeObject(role);

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["editroleUrl"] + "?roleId=" + role.Id.ToString();
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

        public async Task<Role> GetRoleById(string roleId, string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getrolebyIdUrl"] + "?roleId=" + roleId;
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);


            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var roledetail = JsonConvert.DeserializeObject<Role>(result);

            return roledetail;
        }

        public async Task<List<Role>> GetRoleList(string authToken)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getallroleUrl"].ToString();
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var roleData = JsonConvert.DeserializeObject<List<Role>>(result);

            return roleData;
        }
    }
}
