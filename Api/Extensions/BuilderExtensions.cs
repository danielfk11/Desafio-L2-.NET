using DotNetEnv;

namespace api.Extensions
{
    public static class BuilderExtensions
    {
        public static void CarregarAmbiente(this WebApplicationBuilder builder)
        {
            Env.Load();
        }

        public static string ObterVariavel(this string nome)
        {
            return Environment.GetEnvironmentVariable(nome) ?? string.Empty;
        }
    }
}
