using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PathOfServices.API.Swagger
{
    public class AuthAccessTokenOperationFilter : IOperationFilter
    {
        private static bool HasAttribute(MemberInfo methodInfo, Type type, bool inherit)
        {
            // inherit = true also checks inherited attributes
            var actionAttributes = methodInfo.GetCustomAttributes(inherit);
            var controllerAttributes = methodInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes(inherit);
            var actionAndControllerAttributes = controllerAttributes?.Union(actionAttributes) ?? actionAttributes;

            return actionAndControllerAttributes.Any(attr => attr.GetType() == type);
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorizeAttribute = HasAttribute(context.MethodInfo, typeof(AuthorizeAttribute), true);
            var hasAnonymousAttribute = HasAttribute(context.MethodInfo, typeof(AllowAnonymousAttribute), true);

            var isAuthorized = hasAuthorizeAttribute && !hasAnonymousAttribute;

            if (!isAuthorized)
                return;

            operation.Responses.Add("401", new OpenApiResponse{ Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
        }
    }
}
