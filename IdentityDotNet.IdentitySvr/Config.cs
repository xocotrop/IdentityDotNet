using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityDotNet.IdentitySvr
{
    public class Config
    {

        public static List<TestUser> GetUserTests()
        => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "igor",
                Password = "1234"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "teste",
                Password = "1234"
            }
        };

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankOfDotNetApi", "Customer Api for BankOfDotNet")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>{
                new Client{
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"bankOfDotNetApi"}
                },
                //resource owner password grant type
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"bankOfDotNetApi"}
                }
            };
        }
    }
}