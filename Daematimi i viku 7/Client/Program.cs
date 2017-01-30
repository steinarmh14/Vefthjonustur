using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApplication
{   
    //Code from github, slightly modified (https://github.com/IdentityServer/IdentityServer4.Samples/blob/7d768460a2eb6a704a1933ad1f99ca5f384ce8fa/Quickstarts/1_ClientCredentials/src/Client/Program.cs)
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        
        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/api/courses");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);

            var postResponse = await client.PostAsync("http://localhost:5001/api/courses", new StringContent("yolo"));
            if (!postResponse.IsSuccessStatusCode) {
                Console.WriteLine(postResponse.StatusCode);
            }

            var postContent = postResponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine(postContent);

            var idResponse = await client.GetAsync("http://localhost:5001/api/courses/1");
            if (!idResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(idResponse.StatusCode);
            }

            var idContent = idResponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine(idContent);
        }
    }
}
