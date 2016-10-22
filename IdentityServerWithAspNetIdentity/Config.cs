using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerWithAspNetIdentity
{
    public class Config
    {

        // Defining the scope
        // http://identityserver4.readthedocs.io/en/dev/quickstarts/1_client_credentials.html#defining-the-scope
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "api1",
                    Description = "My API"
                }
            };
        }

        // Defining the client
        // http://identityserver4.readthedocs.io/en/dev/quickstarts/1_client_credentials.html#defining-the-client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "mario",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("mariopass".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                }
            };
        }

        // Create a couple of users
        // http://identityserver4.readthedocs.io/en/dev/quickstarts/2_resource_owner_passwords.html#adding-users
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "alice",
                    Password = "password"
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}
