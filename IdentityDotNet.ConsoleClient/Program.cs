using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IdentityDotNet.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {


            var discoRO = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRO.IsError)
            {
                Console.WriteLine(discoRO.Error);
                return;
            }

            var tokenClientRO = new TokenClient(discoRO.TokenEndpoint, "ro.client", "secret");

            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("igor", "1234", "bankOfDotNetApi");
            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return;
            }

            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\r\n");


            /* ------------------- */

            var discovery = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discovery.IsError)
            {
                Console.WriteLine(discovery.Error);
                return;
            }

            var tokenClient = new TokenClient(discovery.TokenEndpoint, "client", "secret");

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\r\n");

            using (var client = new HttpClient())
            {
                client.SetBearerToken(tokenResponse.AccessToken);

                var customerInfo = new StringContent(JsonConvert.SerializeObject(new
                {
                    Id = 10,
                    FirstName = "Igor",
                    LastName = "Janoski dos Santos"
                }), Encoding.UTF8, "application/json");

                var create = await client.PostAsync("http://localhost:5050/api/customers", customerInfo);

                if (!create.IsSuccessStatusCode)
                {
                    Console.WriteLine(create.StatusCode);
                }

                var getCustomerResponse = await client.GetAsync("http://localhost:5050/api/customers");
                if (!getCustomerResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine(getCustomerResponse.StatusCode);
                }
                else
                {
                    var content = await getCustomerResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }

                Console.Read();

            }
        }
    }
}
