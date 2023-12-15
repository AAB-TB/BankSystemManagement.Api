using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace BankSystemManagement.Data.Repos
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly string[] ExcludedPaths =
        {
            "/api/user/login",
            "/api/user/NewAdminRegister",
            "/api/user/allUsers",
            "/api/AccountType/types"
        };

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsExcludedPath(context.Request.Path))
            {
                await _next.Invoke(context);
                return;
            }

            var user = context.User;
            LogUserClaims(user);

            if (HasRequiredRole(user))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }

        private bool IsExcludedPath(PathString path)
        {
            return Array.Exists(ExcludedPaths, excludedPath =>
                path.Equals(excludedPath, StringComparison.OrdinalIgnoreCase));
        }

        private void LogUserClaims(ClaimsPrincipal user)
        {
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }

        private bool HasRequiredRole(ClaimsPrincipal user)
        {
            return user.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer"));
        }
    }
}
