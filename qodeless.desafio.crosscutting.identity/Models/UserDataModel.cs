using qodeless.desafio.domain.Interfaces;

namespace qodeless.desafio.Infra.CrossCutting.identity.Models
{
    public class UserDataModel : IUserDataModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ClaimsType { get; set; }
        public string ClaimsValue { get; set; }
    }
}
