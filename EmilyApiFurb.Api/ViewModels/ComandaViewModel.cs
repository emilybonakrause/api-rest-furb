namespace EmilyApiFurb.Api.ViewModels;

public sealed record ComandaViewModel(
    int UsuarioId,
    string NomeUsuario,
    string TelefoneUsuario,
    IList<ProdutoViewModel> Produtos);