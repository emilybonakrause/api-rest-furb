namespace EmilyApiFurb.Api.ViewModels;

public sealed record AtualizarComandaViewModel(
    int? UsuarioId,
    string? NomeUsuario,
    string? TelefoneUsuario,
    IList<ProdutoViewModel> ProdutosParaAdicionar);