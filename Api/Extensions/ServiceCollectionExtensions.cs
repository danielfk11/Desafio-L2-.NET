using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api.Data;
using api.Services;
using api.Repositories;

namespace api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AdicionarInfraestrutura(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ServicoEmpacotamento>();

            return services;
        }

        public static IServiceCollection AdicionarAplicacao(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddScoped<EmpacotamentoRepository>(); 
            return services;
        }
    }
}
