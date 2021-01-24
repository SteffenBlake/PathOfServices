using System.Security.Claims;

namespace PathOfServices.API.Extensions
{
    /// <summary>
    /// Extends <see cref="ClaimsPrincipal"/>
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        public static string NameIdentifier(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string Name(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }
    }
}
