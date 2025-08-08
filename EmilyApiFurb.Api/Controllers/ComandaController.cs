using EmilyApiFurb.Api.Conversores;
using EmilyApiFurb.Api.ViewModels;
using EmilyApiFurb.Dominio.Contrato;
using EmilyApiFurb.Dominio.Interfaces;
using EmilyApiFurb.Dominio.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmilyApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComandaController : ControllerBase
{
    private readonly IComandaConversor conversor;
    private readonly IComandaServico servico;

    public ComandaController(IComandaServico servico, IComandaConversor conversor)
    {
        this.servico = servico;
        this.conversor = conversor;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodasComandas(CancellationToken cancellationToken)
    {
        try
        {
            var comandas = await servico.ListarComandasAsync(cancellationToken);
            if (comandas is null)
                return NotFound("Nenhuma comanda encontrada.");

            var viewModelRetorno = comandas.Select(conversor.ConverterParaViewModel).ToList();
            return Ok(viewModelRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao obter comanda: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterComandaPorId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("ID da comanda não pode ser vazio.");

        try
        {
            var comanda = await servico.ObterComandaPorIdAsync(id, cancellationToken);
            if (comanda is null)
                return NotFound("Comanda não encontrada.");

            var viewModelRetorno = conversor.ConverterParaViewModel(comanda);
            return Ok(viewModelRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao obter comanda: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CriarComanda([FromBody] ComandaViewModel viewModel, CancellationToken cancellationToken)
    {
        var (ehValido, mensagem) = ValidarComandaViewModel(viewModel);

        if (!ehValido)
            return BadRequest(mensagem);

        var comandaId = Guid.NewGuid();
        var produtos = new List<Produto>();
        foreach (var produtoViewModel in viewModel.Produtos)
        {
            var produto = new Produto(
                comandaId: comandaId,
                codigo: produtoViewModel.Codigo,
                nome: produtoViewModel.Nome,
                preco: produtoViewModel.Preco);

            produtos.Add(produto);
        }

        var comanda = new Comanda(
            id: comandaId,
            usuarioId: viewModel.UsuarioId,
            nomeUsuario: viewModel.NomeUsuario,
            telefoneUsuario: viewModel.TelefoneUsuario,
            produtos: produtos);

        try
        {
            var retorno = await servico.CriarComandaAsync(comanda, cancellationToken);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao criar comanda: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> AtualizarComanda([FromRoute] Guid id, [FromBody] AtualizarComandaViewModel viewModel, CancellationToken cancellationToken)
    {
        var (ehValido, mensagem) = ValidarComandaViewModel(viewModel);

        if (!ehValido)
            return BadRequest(mensagem);

        var contrato = new AtualizarComandaContrato(
            UsuarioId: viewModel.UsuarioId,
            NomeUsuario: viewModel.NomeUsuario,
            TelefoneUsuario: viewModel.TelefoneUsuario,
            ProdutosParaAdicionar: viewModel.ProdutosParaAdicionar.Select(p => new ProdutoContrato(
                Codigo: p.Codigo,
                Nome: p.Nome,
                Preco: p.Preco)).ToList());

        try
        {
            await servico.AtualizarComandaAsync(id, contrato, cancellationToken);
            return Ok("Comanda atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao atualizar comanda: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletarComanda([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("ID da comanda não pode ser vazio.");

        try
        {
            await servico.DeletarComandaAsync(id, cancellationToken);
            return Ok("Comanda deletada com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao deletar comanda: {ex.Message}");
        }
    }

    private (bool ehValido, string mensagem) ValidarComandaViewModel(ComandaViewModel viewModel)
    {
        if (viewModel is null)
            return (false, "O objeto ComandaViewviewModel não pode ser nulo.");

        if (viewModel.UsuarioId <= 0)
            return (false, "O campo UsuarioId deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(viewModel.NomeUsuario))
            return (false, "O campo NomeUsuario é obrigatório.");

        if (viewModel.NomeUsuario.Length > 100)
            return (false, "O campo NomeUsuario não pode exceder 100 caracteres.");

        if (string.IsNullOrWhiteSpace(viewModel.TelefoneUsuario))
            return (false, "O campo TelefoneUsuario é obrigatório.");

        if (!viewModel.TelefoneUsuario.All(char.IsDigit) || viewModel.TelefoneUsuario.Length != 11)
            return (false, "O campo TelefoneUsuario deve conter exatamente 11 dígitos numéricos.");

        if (viewModel.Produtos is null || !viewModel.Produtos.Any())
            return (false, "É necessário informar pelo menos um produto na comanda.");

        if (viewModel.Produtos.Count == 0 || viewModel.Produtos is null)
            return (false, "A lista de produtos não pode ser nula ou vazia.");

        var codigosDuplicados = viewModel.Produtos
            .GroupBy(p => p.Codigo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (codigosDuplicados.Any())
            return (false, $"Não é permitido adicionar o mesmo produto (Codigo: {codigosDuplicados.First()}) mais de uma vez.");


        for (int i = 0; i < viewModel.Produtos.Count; i++)
        {
            var p = viewModel.Produtos[i];
            if (p is null)
                return (false, $"O produto na posição {i} é nulo.");

            if (p.Codigo <= 0)
                return (false, $"O Código do produto na posição {i} é inválido.");

            if (string.IsNullOrWhiteSpace(p.Nome))
                return (false, $"O Nome do produto na posição {i} é obrigatório.");

            if (p.Preco < 0)
                return (false, $"O Preço do produto na posição {i} não pode ser negativo.");
        }

        return (true, string.Empty);
    }

    private (bool ehValido, string mensagem) ValidarComandaViewModel(AtualizarComandaViewModel viewModel)
    {
        if (viewModel is null)
            return (false, "O objeto AtualizarComandaViewModel não pode ser nulo.");

        if (viewModel.UsuarioId is not null && viewModel.UsuarioId <= 0)
            return (false, "O campo UsuarioId deve ser maior que zero.");

        if (viewModel.NomeUsuario is not null && viewModel.NomeUsuario.Length > 100)
            return (false, "O campo NomeUsuario não pode exceder 100 caracteres.");

        if (viewModel.TelefoneUsuario is not null && (!viewModel.TelefoneUsuario.All(char.IsDigit) || viewModel.TelefoneUsuario.Length != 11))
            return (false, "O campo TelefoneUsuario deve conter exatamente 11 dígitos numéricos.");

        if (viewModel.ProdutosParaAdicionar is not null && viewModel.ProdutosParaAdicionar.Count > 0)
        {
            var codigosDuplicados = viewModel.ProdutosParaAdicionar
                 .GroupBy(p => p.Codigo)
                 .Where(g => g.Count() > 1)
                 .Select(g => g.Key)
                 .ToList();

            if (codigosDuplicados.Any())
                return (false, $"Não é permitido adicionar o mesmo produto (Codigo: {codigosDuplicados.First()}) mais de uma vez.");
        }

        for (int i = 0; i < viewModel.ProdutosParaAdicionar.Count; i++)
        {
            var p = viewModel.ProdutosParaAdicionar[i];
            if (p is null)
                return (false, $"O produto na posição {i} é nulo.");

            if (p.Codigo <= 0)
                return (false, $"O Código do produto na posição {i} é inválido.");

            if (string.IsNullOrWhiteSpace(p.Nome))
                return (false, $"O Nome do produto na posição {i} é obrigatório.");

            if (p.Preco < 0)
                return (false, $"O Preço do produto na posição {i} não pode ser negativo.");
        }

        return (true, string.Empty);
    }
}
