using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RestApiFurb.Infra.Contextos;

public class ContextoFactory : IDesignTimeDbContextFactory<Contexto>
{
    public Contexto CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Contexto>();
        optionsBuilder.UseSqlServer(
            "Server=Emily;Database=EmilyFurb;Trusted_Connection=True;TrustServerCertificate=True;",
            sql => sql.MigrationsAssembly(
                typeof(Contexto).Assembly.FullName
            )
        );

        return new Contexto(optionsBuilder.Options);
    }
}
