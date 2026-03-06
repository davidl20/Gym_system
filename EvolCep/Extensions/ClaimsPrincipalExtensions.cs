using System.Security.Claims;

namespace EvolCep.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetIdClient (this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("ClientId");

            if (claim == null)
                throw new Exception("ClientId no encontrado en el token");

            return int.Parse(claim.Value);
        }
    }
}
