using EmilyApiFurb.Dominio.Contrato;
using EmilyApiFurb.Dominio.Interfaces;
using EmilyApiFurb.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;

namespace EmilyApiFurb.Servicos.Servicos;

internal sealed class ComandaServico : IComandaServico
{
    private readonly IRepositorioBase<Comanda> repositorio;
    private readonly IRepositorioBase<Produto> repositorioProduto;

    public ComandaServico(IRepositorioBase<Comanda> repositorio, IRepositorioBase<Produto> repositorioProduto)
    {
        this.repositorio = repositorio;
        this.repositorioProduto = repositorioProduto;
    }

    public async Task<Comanda> CriarComandaAsync(Comanda comanda, CancellationToken cancellationToken)
    {
        await repositorio.SalvarAsync(comanda, cancellationToken);

        return comanda;
    }

    public async Task<IList<Comanda>> ListarComandasAsync(CancellationToken cancellationToken = default)
    {
        return await repositorio
            .MontarConsulta()
            .Include(x => x.Produtos)
            .ToListAsync(cancellationToken);
    }

    public async Task<Comanda> ObterComandaPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID da comanda não pode ser vazio.", nameof(id));

        return await repositorio
            .MontarConsulta()
            .Include(x => x.Produtos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AtualizarComandaAsync(Guid Id, AtualizarComandaContrato contrato, CancellationToken cancellationToken = default)
    {
        var comandaExistente = await repositorio
            .MontarConsulta()
            .Include(x => x.Produtos)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);

        if (comandaExistente is null)
            throw new KeyNotFoundException($"Comanda com ID {Id} não encontrada.");

        foreach (var produtoContrato in contrato.ProdutosParaAdicionar)
        {
            var produtoExistente = comandaExistente.Produtos.Where(x => x.Codigo == produtoContrato.Codigo).FirstOrDefault();

            if (produtoExistente is not null)
            {
                produtoExistente.AtualizarProduto(produtoContrato);
                await repositorioProduto.AtualizarAsync(produtoExistente, cancellationToken);
            }
            else
            {
                var produtoParaAdicionar = new Produto(
                    comandaId: Id,
                    codigo: produtoContrato.Codigo,
                    nome: produtoContrato.Nome,
                    preco: produtoContrato.Preco);

                await repositorioProduto.SalvarAsync(produtoParaAdicionar, cancellationToken);
            }
        }

        comandaExistente.AtualizarComanda(contrato);
        await repositorio.AtualizarAsync(comandaExistente, cancellationToken);
    }

    public async Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await repositorio.DeletarAsync(id, cancellationToken);
    }
}