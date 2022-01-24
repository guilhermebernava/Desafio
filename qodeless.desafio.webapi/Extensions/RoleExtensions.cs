namespace qodeless.desafio.webapi.Extensions
{
    public static class RoleExtensions
    {
        public static bool IsValidRole(this string role)
        {
            switch (role)
            {  
                case "SUPER_ADMIN":
                    return true;
            }
            return false;
        }
    }
}
