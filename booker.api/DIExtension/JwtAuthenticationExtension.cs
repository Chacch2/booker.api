using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace booker.api.DIExtension
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authSection = configuration.GetSection("JwtSettings:JwtOptions");
            var issuer = authSection["Issuer"];
            var audience = authSection["Audience"];
            var secretKey = authSection["Secret"];

            // 驗證必要設定
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT Secret key is not configured.");
            }
            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new InvalidOperationException("JWT Issuer is not configured.");
            }
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT Audience is not configured.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? string.Empty)),
                        ClockSkew = TimeSpan.Zero
                    };
                });


            return services;
        }
    }
}
