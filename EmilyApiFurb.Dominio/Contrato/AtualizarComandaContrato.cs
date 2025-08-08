namespace EmilyApiFurb.Dominio.Contrato;

public sealed record AtualizarComandaContrato(
    int? UsuarioId,
    string? NomeUsuario,
    string? TelefoneUsuario,
    IList<ProdutoContrato> ProdutosParaAdicionar);