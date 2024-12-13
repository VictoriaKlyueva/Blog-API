using BackendLaboratory.Data.Database;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BackendLaboratory.Service.IService;

namespace BackendLaboratory.Util.Token
{
    public class TokenBlackListPolicy : AuthorizationHandler<TokenBlackListRequirment>
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenBlackListPolicy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenBlackListRequirment requirement)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tokenBlacklistService = scope.ServiceProvider.GetRequiredService<ITokenBlacklistService>();
                string authorizationHeader = _serviceProvider
                    .GetRequiredService<IHttpContextAccessor>()
                    .HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    var token = authorizationHeader.Substring(AppConstants.Bearer.Length).Trim();

                    // Проверка, находится ли токен в черном списке
                    if (await tokenBlacklistService.IsTokenBlacklistedAsync(token))
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
        }
    }
}

