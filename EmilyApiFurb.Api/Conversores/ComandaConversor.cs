using EmilyApiFurb.Api.ViewModels;
using EmilyApiFurb.Dominio.Modelos;

namespace EmilyApiFurb.Api.Conversores;

internal sealed class ComandaConversor : IComandaConversor
{
    public ComandaViewModel ConverterParaViewModel(Comanda comanda)
    {
        return new ComandaViewModel(
            UsuarioId: comanda.UsuarioId,
            NomeUsuario: comanda.NomeUsuario,
            TelefoneUsuario: comanda.TelefoneUsuario,
            Produtos: comanda.Produtos.Select(p => new ProdutoViewModel(
                Codigo: p.Codigo,
                Nome: p.Nome,
                Preco: p.Preco)).ToList());
    }
}
