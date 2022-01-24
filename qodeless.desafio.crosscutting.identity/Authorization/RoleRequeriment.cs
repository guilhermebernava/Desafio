using Microsoft.AspNetCore.Authorization;

namespace qodeless.desafio.crosscutting.identity.Authorization
{
    public class RoleRequeriment : IAuthorizationRequirement
    {
        public string Name { get; set; }
        public RoleRequeriment(string name)
        {
            Name = name;
        }
    }
}
