using EmilyApiFurb.Dominio.Contrato;
using EmilyApiFurb.Dominio.Modelos.Base;

namespace EmilyApiFurb.Dominio.Modelos;

public class Produto : ModeloBase
{
    public Guid ComandaId { get; private set; }
    public int Codigo { get; private set; }
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }

    public Produto()
    { }

    public Produto(
        Guid comandaId,
        int codigo,
        string nome,
        decimal preco)
    {
        ComandaId = comandaId;
        Codigo = codigo;
        Nome = nome;
        Preco = preco;
    }

    public void AtualizarProduto(ProdutoContrato contrato)
    {
        Codigo = contrato.Codigo;
        Nome = contrato.Nome;
        Preco = contrato.Preco;
    }
}