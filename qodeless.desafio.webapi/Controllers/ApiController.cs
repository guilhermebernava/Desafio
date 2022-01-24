using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.Infra.CrossCutting.identity.Data;
using qodeless.desafio.Infra.CrossCutting.identity.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace qodeless.desafio.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected readonly ApplicationDbContext Db;
        public ApiController(ApplicationDbContext db,
            IWebHostEnvironment env = null)

        {
            Db = db;
        }

        protected new IActionResult Response(object result = null, bool success = true, string errorMessage = "")
        {
            if (success)
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = errorMessage
            });
        }
        protected new IActionResult ModelStateError()
        {
            var result = new List<string>();
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                result.Add(erroMsg);
            }

            return Response(success: false, errorMessage: string.Join("\r\n", result));
        }
        protected IUserDataModel GetLoggedUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var email = claimsIdentity.FindFirst("/email")?.Value;

            return GetUser(email);
        }
        private readonly ICollection<string> _errors = new List<string>();
        protected ActionResult CustomResponse(object result = null)
        {
            if (IsOperationValid())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {
                    "ErrorMessages",
                    _errors.ToArray()
                }
            }));
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(c => c.Errors))
            {
                AddError(error.ErrorMessage);
            }
            return CustomResponse(new { success = true });
        }

        protected List<ClaimViewModel> GetDefaultClaims(string roleId, IUserDataModel user)
        {
            var result = new List<ClaimViewModel>();
            var roleClaims = Db.RoleClaims.Where(_ => _.RoleId == roleId).ToList();
            var userClaims = Db.UserClaims.Where(_ => _.UserId == user.UserId).ToList();

            if (userClaims != null && userClaims.Count > 0)
            {
                //Adiciona claims relacionados ao User sem repetir o que ja foi adicionado anteriormente
                foreach (var subItem in userClaims.Where(_ => !result.Any(r => r.ClaimType.ToLower() == _.ClaimType.ToLower() && r.ClaimValue.ToLower() == _.ClaimValue.ToLower())))
                {
                    result.Add(new ClaimViewModel { ClaimType = subItem.ClaimType, ClaimValue = subItem.ClaimValue });
                }
                return result;
            }

            if (roleClaims != null && roleClaims.Count > 0)
            {
                //Adiciona claims relacionados ao Role
                foreach (var item in roleClaims)
                {
                    result.Add(new ClaimViewModel { ClaimType = item.ClaimType, ClaimValue = item.ClaimValue });
                }
            }

            return result;
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError(error.PropertyName, error.ErrorMessage);
            }
            return CustomResponse(new { success = true });
        }
        protected bool IsOperationValid() => !_errors.Any();
        protected void AddError(string field, string erro) => _errors.Add($"{field}|{erro}");
        protected void AddError(string erro) => _errors.Add(erro);
        protected void ClearErrors() => _errors.Clear();

        #region MAIN_QUERIES


        protected IUserDataModel GetUser(string email)
        {
            var user = Db.Users.Where(_=> _.Email == email).FirstOrDefault();
            var t = Db.UserRoles.Where(_=> _.UserId == user.Id).FirstOrDefault();

            if (t != null)
            {
                var teste = Db.Roles.Where(_=> _.Id == t.RoleId).FirstOrDefault();
                return new UserDataModel() { UserId = user.Id, Email = user.Email, RoleId = t.RoleId, Role = teste.Name};
            }

            return new UserDataModel();

        }

        #endregion //MAIN_QUERIES
    }
}
