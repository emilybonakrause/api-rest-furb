using EmilyApiFurb.Dominio.Contrato;
using EmilyApiFurb.Dominio.Modelos.Base;

namespace EmilyApiFurb.Dominio.Modelos;

public class Comanda : ModeloBase
{
    public int UsuarioId { get; private set; }
    public string NomeUsuario { get; private set; }
    public string TelefoneUsuario { get; private set; }
    public IList<Produto> Produtos { get; private set; }

    public Comanda()
    { }

    public Comanda(
        Guid id,
        int usuarioId,
        string nomeUsuario,
        string telefoneUsuario,
        IList<Produto> produtos)
    {
        Id = id;
        UsuarioId = usuarioId;
        Produtos = produtos;
        NomeUsuario = nomeUsuario;
        TelefoneUsuario = telefoneUsuario;
    }

    public void AtualizarComanda(AtualizarComandaContrato contrato)
    {
        if (contrato.UsuarioId is not null)
            UsuarioId = contrato.UsuarioId.Value;

        if (!string.IsNullOrWhiteSpace(contrato.NomeUsuario))
            NomeUsuario = contrato.NomeUsuario;

        if (!string.IsNullOrWhiteSpace(contrato.TelefoneUsuario))
            TelefoneUsuario = contrato.TelefoneUsuario;
    }

    public void AtualizarUsuarioId(int UsuarioId)
    {
        this.UsuarioId = UsuarioId;
    }
}