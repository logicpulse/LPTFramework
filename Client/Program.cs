using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync() {

            //Requesting a token using client credentials grant
            TokenResponse tokenResponse = await TestClientCredentialsGrant();

            //Requesting a token using the password grant
            //TokenResponse tokenResponse = await TestRequestingTokenPasswordGrant();

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                PressAnyKey();
            }

            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(JArray.Parse(content));

            PressAnyKey();
        }

        //Requesting a token using client credentials grant
        private static async Task<TokenResponse> TestClientCredentialsGrant()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                PressAnyKey();
                return null;
            }

            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }

        //Requesting a token using the password grant
        private static async Task<TokenResponse> TestRequestingTokenPasswordGrant()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            //Requesting a token using the password grant
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }

        private static void PressAnyKey()
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
