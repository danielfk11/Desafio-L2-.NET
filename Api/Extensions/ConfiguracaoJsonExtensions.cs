using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace api.Extensions;

public static class ConfiguracaoJsonExtensions
{
    public static IMvcBuilder ConfigurarSerializacaoJson(this IServiceCollection services)
    {
        return services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
    }
}
