using EmilyApiFurb.Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using RestApiFurb.Infra.Contextos;
using RestApiFurb.Infra.Repositorio;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarBancoDeDados(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Contexto>(options => options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));

        return services;
    }
}