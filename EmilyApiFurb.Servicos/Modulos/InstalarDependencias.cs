using EmilyApiFurb.Dominio.Interfaces;
using EmilyApiFurb.Servicos.Servicos;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services, string connectionString)
    {
        services.AdicionarBancoDeDados(connectionString);

        services.AddScoped<IComandaServico, ComandaServico>();

        return services;
    }
}