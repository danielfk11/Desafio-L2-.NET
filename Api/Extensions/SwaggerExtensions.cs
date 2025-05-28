using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AdicionarSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API de Empacotamento",
                    Version = "v1",
                    Description = "Documentação da API de Empacotamento"
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira: Bearer {seu_token_jwt}",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            });

            return services;
        }

        public static WebApplication UsarSwaggerPersonalizado(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Empacotamento v1");
            });

            return app;
        }
    }
}
