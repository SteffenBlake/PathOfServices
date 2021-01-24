using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PathOfServices.Business;
using PathOfServices.Business.Configuration;
using PathOfServices.Business.Database;
using PathOfServices.Business.Models;

namespace PathOfServices.API.Controllers
{
    public class OAuthController : PathOfServicesControllerBase
    {
        private PathOfServicesConfig Config { get; }
        private IMemoryCache Memory { get; }
        private PathOfServicesDbContext DBContext { get; }
        private UserManager<UserEntity> UserManager { get; }

        public OAuthController(PathOfServicesConfig config, IMemoryCache memory, PathOfServicesDbContext dbContext, UserManager<UserEntity> userManager)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Memory = memory;
            DBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("Authorize")]
        [AllowAnonymous]
        public RedirectResult Authorize()
        {
            // Generate a random 16char GUID nonce to validate on the callback
            var nonce = Guid.NewGuid().ToString().Replace("-", "");

            Memory.Set(nonce, nonce, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Config.PathOfExileApi.NonceExpirySeconds),
                Size = nonce.Length
            });

            var authUri = UriPath.Combine(Config.PathOfExileApi.Route, "/OAuth/Authorize");

            var redirectUri = Url.Action("AuthorizeCallback", "OAuth", null, Request.Scheme);
            // Build up query params to append to the provided redirect Uri
            var queryParams = new Dictionary<string, string>
            {
                {"client_id", Config.PathOfExileApi.ClientId},
                {"response_type", "code"},
                {"scope", "profile"},
                {"redirect_uri", redirectUri},
                {"state", nonce}
            };

            var authQuery = QueryHelpers.AddQueryString(authUri, queryParams);

            return Redirect(authQuery);
        }


        [HttpGet("Authorize-Callback")]
        [AllowAnonymous]
        public async Task<RedirectResult> AuthorizeCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(state) || !Memory.TryGetValue(state, out _))
            {
                // Unrecognized Nonce, go back home
                return BackToHome();
            }

            // Delete the nonce now, we are done with it
            Memory.Remove(state);

            // Check if we already recognize this code, if not, build it
            var codeEntity = await DBContext.OAuthCodes.FindAsync(code);

            if (codeEntity == null)
            {
                codeEntity = new OAuthCodeEntity
                {
                    Value = code
                };
                await DBContext.AddAsync(codeEntity);
                await DBContext.SaveChangesAsync();
            }

            var token = string.IsNullOrEmpty(codeEntity.UserId) ? null : 
                DBContext
                    .OAuthTokens
                    .FirstOrDefault(t =>
                        t.UserId == codeEntity.UserId && (t.Expiry == null || t.Expiry.Value > DateTime.Now)
                    );

            // This user already has a valid access token, lets just use that instead
            if (token != null)
            {
                return BackToHome(token);
            }

            // This is a new, unrecognized user, try and fetch an access token

            var tokenUri = UriPath.Combine(Config.PathOfExileApi.Route, "/OAuth/Token");

            var redirectUri = Url.Action("TokenCallback", "OAuth", null, Request.Scheme);
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", Config.PathOfExileApi.ClientId },
                { "client_secret", Config.PathOfExileApi.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                {"redirect_uri", redirectUri }
            };

            var tokenQuery = QueryHelpers.AddQueryString(tokenUri, queryParams);

            return Redirect(tokenQuery);
        }

        [HttpGet("Token-Callback")]
        [AllowAnonymous]
        public async Task<RedirectResult> TokenCallback(string access_token, long? expires_in)
        {
            var token = await EnsureUserAsync(access_token);

            if (expires_in.HasValue)
            {
                token.Expiry = DateTime.Now.AddSeconds(expires_in.Value);
            }

            await DBContext.AddAsync(token);
            await DBContext.SaveChangesAsync();

            return BackToHome(token);
        }

        [NonAction]
        private RedirectResult BackToHome(OAuthTokenEntity token = null)
        {
            if (token == null)
                return Redirect(Config.Origin);

            var cookieOptions = new CookieOptions
            {
                Path = "/",
                IsEssential = true,
                Expires = token.Expiry,
            };

            Response.Cookies.Append("access_token", token.Value, cookieOptions);

            return Redirect(Config.Origin);
        }

        [NonAction]
        private async Task<OAuthTokenEntity> EnsureUserAsync(string access_token)
        {
            var token = await DBContext.OAuthTokens.SingleOrDefaultAsync(t => t.Value == access_token && (t.Expiry == null || t.Expiry.Value > DateTime.Now));

            // User exists, short circuit out
            if (!string.IsNullOrEmpty(token?.UserId))
                return token;

            var profileUri = UriPath.Combine(Config.PathOfExileApi.Route, "/profile");
            var profileQuery = QueryHelpers.AddQueryString(profileUri, nameof(access_token), access_token);

            using var client = new HttpClient();
            using var response = await client.GetAsync(profileQuery);

            if (!response.IsSuccessStatusCode)
                throw new AuthenticationException(response.ReasonPhrase);

            var contentRaw = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileResponse>(contentRaw);

            if (string.IsNullOrEmpty(profile?.Name))
                throw new AuthenticationException();

            var user = await UserManager.FindByNameAsync(profile.Name);

            if (user == null)
            {
                user = new UserEntity
                {
                    UserName = profile.Name,
                    Name = profile.Name,
                    Realm = profile.Realm,
                    UUID = profile.UUID
                };

                var result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new AuthenticationException(result.Errors.First().Description);
            }

            token = new OAuthTokenEntity
            {
                Value = access_token,
                UserId = user.Id
            };

            return token;
        }
    }
}
