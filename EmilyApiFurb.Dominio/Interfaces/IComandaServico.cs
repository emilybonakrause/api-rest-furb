using EmilyApiFurb.Dominio.Contrato;
using EmilyApiFurb.Dominio.Modelos;
using System.Diagnostics.Contracts;

namespace EmilyApiFurb.Dominio.Interfaces;

public interface IComandaServico
{
    Task<Comanda> CriarComandaAsync(Comanda comanda, CancellationToken cancellationToken = default);

    Task<IList<Comanda>> ListarComandasAsync(CancellationToken cancellationToken = default);

    Task<Comanda> ObterComandaPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AtualizarComandaAsync(Guid Id, AtualizarComandaContrato contrato, CancellationToken cancellationToken = default);

    Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken = default);
}