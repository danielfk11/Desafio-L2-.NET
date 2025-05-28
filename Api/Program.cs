using api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.CarregarAmbiente();

var connectionString = "CONNECTION_STRING".ObterVariavel();
var ambiente = "AMBIENTE".ObterVariavel().ToLower();
var jwtSecret = "JWT_SECRET".ObterVariavel();

builder.Services
    .AdicionarInfraestrutura(connectionString)
    .AdicionarAplicacao()
    .ConfigurarAutenticacao(jwtSecret)
    .AdicionarSwagger()
    .ConfigurarSerializacaoJson();

var app = builder.Build();

app.AplicarMigracoes();

if (ambiente == "dev")
{
    app.UsarSwaggerPersonalizado();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
