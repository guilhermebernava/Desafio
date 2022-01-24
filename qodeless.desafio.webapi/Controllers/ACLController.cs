using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using qodeless.desafio.Infra.CrossCutting.identity.Data;
using qodeless.desafio.Infra.CrossCutting.identity.Models.AccountModels;
using qodeless.desafio.webapi.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace qodeless.desafio.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ACLController : ApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ACLController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
          ApplicationDbContext db, IWebHostEnvironment env) : base(db, env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleViewModel vm)
        {
            if (vm.Role.ToUpper().IsValidRole())
            {
                if (!_roleManager.RoleExistsAsync(vm.Role).Result)
                {
                    await _roleManager.CreateAsync(new IdentityRole(vm.Role.ToUpper()));
                }
                var role = await _roleManager.FindByNameAsync(vm.Role);
                return Response(true);
            }
            return Response(false);
        }

        [HttpGet("Users")]
        public async Task<List<IdentityUser>> GetUsers()
        {
            return Db.Users.ToList();

        }


        [HttpPut("UpdatePlayerLockoutEnebledById")]
        public async Task<IActionResult> UpdatePlayer(string id, bool eneable)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.LockoutEnabled = eneable;
            await _userManager.UpdateAsync(user);
            return Response($"state updated for {user.LockoutEnabled}");
        }

        [HttpGet("Roles")]
        public async Task<List<IdentityRole>> GetRoles()
        {
            return Db.Roles.ToList();
        }

        [HttpGet("UserRoles")]
        public async Task<List<IdentityUserRole<string>>> GetUserRoles()
        {
            return Db.UserRoles.ToList();
        }

        [HttpGet("UserClaims")]
        public async Task<List<IdentityUserClaim<string>>> GetUserClaims()
        {
            return Db.UserClaims.ToList();
        }

        [HttpGet("UserClaimsPlayers")]
        public async Task<List<IdentityUserClaim<string>>> GetRoleClaims()
        {
            return Db.UserClaims.ToList();
        }


        [HttpPost("AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] RegisterViewModel vm)
        {

            var user = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email,
            };
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                if (vm.Role.ToUpper().IsValidRole())
                {
                    if (!_roleManager.RoleExistsAsync(vm.Role).Result)
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole(vm.Role.ToUpper()));
                    }
                    await _userManager.AddToRoleAsync(user, vm.Role);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                return Response(true);
            }
            return Response(false);
        }
    }
}
