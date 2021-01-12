using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathOfServices.Business;
using PathOfServices.Business.Database;

namespace PathOfServices.API.Middleware
{
    public class OAuthTokenMiddleWare : IMiddleware
    {
        private PathOfServicesDbContext DBContext { get; }
        private UserManager<UserEntity> UserManager { get; }

        public OAuthTokenMiddleWare(PathOfServicesDbContext dbContext, UserManager<UserEntity> userManager)
        {
            DBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await TryAuthorizeAsync(context);
            await next(context);
        }

        private async Task TryAuthorizeAsync(HttpContext context)
        {
            try
            {
                var tokenVal = context.Request.Query["access_token"].First();

                var token = DBContext.OAuthTokens.Include(t => t.User)
                    .SingleOrDefault(t => t.Value == tokenVal);

                if (token == null)
                    return;

                // Identity Principal
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, token.User.UserName),
                    new Claim(ClaimTypes.NameIdentifier, token.User.UserName),
                    new Claim(OauthClaimTypes.AccessToken, token.Value),
                };

                foreach (var role in await UserManager.GetRolesAsync(token.User))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "basic");
                context.User = new ClaimsPrincipal(identity);
            }
            catch
            {
            }
        }
    }
}
