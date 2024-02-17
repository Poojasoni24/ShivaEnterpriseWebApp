using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Interface;
using System.ComponentModel.Design;
using System.Text;

namespace ShivaEnterpriseWebApp.Services.Implementation
{
    public class LoginServiceImpl: ILoginServiceImpl
    {
        private readonly JObject urlCollections;
        public LoginServiceImpl()
        {
            urlCollections = JObject.Parse(File.ReadAllText("systemConfigurations.json"));
        }

        public async Task<UserDetails> GetUserdetail(string userName)
        {
            var url = urlCollections["baseUrl"].ToString() + urlCollections["getUserDetail"] + "?userName=" + userName.ToString();
            var client = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("GET"), url);

            //Pass in the full URL and the json string content
            var response = await client.SendAsync(request);

            //It would be better to make sure this request actually made it through
            var result = await response.Content.ReadAsStringAsync();

            //close out the client
            client.Dispose();

            var users = JsonConvert.DeserializeObject<UserDetails>(result);

            return users;
        }
        public async Task<AuthDAOs> PerformLogin(LoginModel loginModel)
        {
            try
            {
                string json = "{ \"Username\": \"" + loginModel.Username + "\" , \"Password\":\""+loginModel.Password+"\"}"; ;
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = urlCollections["baseUrl"].ToString() + urlCollections["authUrl"].ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                request.Content = data;
                //Pass in the full URL and the json string content
                var response = await client.SendAsync(request);

                //It would be better to make sure this request actually made it through
                var result = await response.Content.ReadAsStringAsync();

                //close out the client
                client.Dispose();
                var authDao = JsonConvert.DeserializeObject<AuthDAOs>(result);

                return authDao;

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<string> PerformLogout()
        {
            try
            {
                var url = urlCollections["baseUrl"].ToString() + urlCollections["logOutUrl"].ToString();
                var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                //Pass in the full URL and the json string content
                var response = await client.SendAsync(request);

                //It would be better to make sure this request actually made it through
                var result = await response.Content.ReadAsStringAsync();

                //close out the client
                client.Dispose();
                var authDao = JsonConvert.DeserializeObject<AuthDAOs>(result);

                return result;

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

    }
}
