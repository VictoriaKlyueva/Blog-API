using BackendLaboratory.Repository.IRepository;

namespace BackendLaboratory.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenBlacklistMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var tokenBlacklistRepository = scope.ServiceProvider.GetRequiredService<ITokenBlacklistRepository>();

                if (context.Request.Headers.TryGetValue("Authorization", out var token))
                {
                    token = token.ToString().Replace("Bearer ", string.Empty);
                    if (await tokenBlacklistRepository.IsBlacklisted(token))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has been blacklisted.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
