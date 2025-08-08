using EmilyApiFurb.Dominio.Interfaces;
using EmilyApiFurb.Dominio.Modelos.Base;
using Microsoft.EntityFrameworkCore;
using RestApiFurb.Infra.Contextos;

namespace RestApiFurb.Infra.Repositorio;

internal sealed class RepositorioBase<T> : IRepositorioBase<T> where T : ModeloBase
{
    private readonly DbSet<T> dbSet;
    private readonly Contexto contexto;

    public RepositorioBase(Contexto contexto)
    {
        this.contexto = contexto;
        dbSet = contexto.Set<T>();
    }

    public async Task<T> ObterPorIdAsync(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public IQueryable<T> MontarConsulta()
    {
        return dbSet.AsQueryable();
    }

    public async Task SalvarAsync(T modelo, CancellationToken cancellationToken)
    {
        await dbSet.AddAsync(modelo);
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(T modelo, CancellationToken cancellationToken)
    {
        dbSet.Update(modelo);
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletarAsync(Guid id, CancellationToken cancellationToken)
    {
        var entidade = await dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entidade == null)
            throw new KeyNotFoundException($"Entidade do tipo {typeof(T).Name} com id {id} não encontrada.");

        dbSet.Remove(entidade);

        await contexto.SaveChangesAsync(cancellationToken);
    }
}
