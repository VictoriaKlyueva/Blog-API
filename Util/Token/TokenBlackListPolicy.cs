using BackendLaboratory.Data.Database;
using Microsoft.AspNetCore.Authorization;

namespace BackendLaboratory.Util.Token
{
    public class TokenBlackListPolicy : AuthorizationHandler<TokenBlackListRequirment>
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenBlackListPolicy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenBlackListRequirment requirement)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                string authorizationHeader = _serviceProvider
                    .GetRequiredService<IHttpContextAccessor>()
                    .HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    var token = authorizationHeader.Substring("Bearer ".Length);
                    var blackToken = appDbContext.BlackTokens.FirstOrDefault(b => b.Blacktoken == token);

                    if (blackToken != null)
                    {
                        context.Fail();
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }
}
