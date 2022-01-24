using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qodeless.desafio.crosscutting.identity.Authorization
{
    public class RoleRequerimentHandler : AuthorizationHandler<RoleRequeriment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequeriment requirement)
        {
            var role = context.User.IsInRole(requirement.Name);

            if (role)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
