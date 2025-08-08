using EmilyApiFurb.Dominio.Modelos.Base;

namespace EmilyApiFurb.Dominio.Interfaces;

public interface IRepositorioBase<T> where T : ModeloBase
{
    IQueryable<T> MontarConsulta();

    Task<T> ObterPorIdAsync(Guid id);

    Task SalvarAsync(T modelo, CancellationToken cancellationToken);

    Task AtualizarAsync(T modelo, CancellationToken cancellationToken);

    Task DeletarAsync(Guid id, CancellationToken cancellationToken);
}
