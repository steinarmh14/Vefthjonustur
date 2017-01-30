using System.Collections;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using System.Security.Claims;

public class Config
{
	public static IEnumerable<Scope> GetScopes()
	{
		return new List<Scope>
		{
			new Scope
			{
				Name = "api1",
				Description = "My API",
				IncludeAllClaimsForUser = true
			}
		};
	}

	public static List<InMemoryUser> GetUsers()
	{
		return new List<InMemoryUser>
		{
			new InMemoryUser
			{
				Subject = "1",
				Username = "alice",
				Password = "password",
				Claims = new Claim[]
				{
					new Claim("IsTeacher", "true"),
				}
			},
			new InMemoryUser
			{
				Subject = "2",
				Username = "bob",
				Password = "password",
				Claims = new Claim[]
				{
					new Claim("IsStudent", "true"),
				}
			}
		};
	}

	public static List<Client> GetClients()
	{
		return new List<Client>
		{
			// other clients omitted...

			// resource owner password grant client
			new Client
			{
				ClientId = "ro.client",
				AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

				ClientSecrets = new List<Secret>
				{
					new Secret("secret".Sha256())
				},
				AllowedScopes = new List<string>
				{
					"api1"
				}
			}
		};
	}
}