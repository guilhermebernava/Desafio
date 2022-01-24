using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.Infra.CrossCutting.identity.Data;
using qodeless.desafio.Infra.CrossCutting.identity.Models.AccountModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace qodeless.desafio.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSetup _jwtSetup;


        public AuthController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<JWTSetup> jwtSetup
            ) : base(db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSetup = jwtSetup.Value;
        }


        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel vm)
        {
            if (!ModelState.IsValid) return ModelStateError();

            var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, false, true);

            if (result.Succeeded)
            {
                var user = GetUser(vm.Email);

                if (user == null)
                {
                    return Response(success: false, errorMessage: "invalid User");
                }

                var token = await GenerateJWTToken(user);
                return Response(
                    new
                    {
                        token = token,
                        user = user
                    }
                );
            }

            return Response(success: false, errorMessage: "invalid user or password");
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUser()
        {
            return Response(new { user = GetUser(User.Identity.Name) });
        }

        private async Task<string> GenerateJWTToken(IUserDataModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetup.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                }),

                Issuer = _jwtSetup.Emiter,
                Audience = _jwtSetup.ValidOn,
                Expires = DateTime.UtcNow.AddHours(_jwtSetup.Duration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var claims = GetDefaultClaims(user.RoleId, user);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
                }
            }
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
