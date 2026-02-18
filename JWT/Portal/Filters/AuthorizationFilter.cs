using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portal.Services;

namespace Portal.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly SecurityProvider securityProvider;
        public AuthorizationFilter(SecurityProvider securityProvider)
        {
            this.securityProvider = securityProvider;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //If header does not include authorization field
            if(!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                //Stop the pipeline and return a 401 unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            var authorization = token.ToString();

            //If authorization field does not start with bearer
            if (!authorization.StartsWith("Bearer")) {
                //Stop the pipeline and return a 401 unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            authorization = authorization[7..];
            
            //If authorization bearer token is invalid
            if(!securityProvider.ValidateToken(authorization))
            {
                //Stop the pipeline and return a 401 unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }



        }
    }
}
