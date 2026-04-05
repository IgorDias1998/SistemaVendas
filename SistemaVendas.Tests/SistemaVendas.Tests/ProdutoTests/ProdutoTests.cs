using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Repositories;

public class ProdutoTests
{
    [Fact(DisplayName = "Gera excecao ao criar um produto com preco negativo.")]
    public void CriarProduto_ComPrecoNegativo_DeveLancarErro()
    {
        var nome = "Produto Teste";
        var descricao = "Descricao do produto teste";
        var preco = -10.0m;
        var estoque = 5;
        var codigo = "PROD123";

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Produto(nome, descricao, preco, estoque, codigo));

        Assert.Equal("Preço não pode ser negativo. (Parameter 'preco')", ex.Message);
    }

    [Fact(DisplayName = "Criar produto com titulo nulo nao deve ser aceito.")]
    public void CriarProduto_ComTituloNulo_DeveFalharNoValidator()
    {
        var dto = new ProdutoCriarDto
        {
            TituloProduto = null,
            PrecoProduto = 10,
            EstoqueProduto = 5,
            CodigoProduto = "ABC123"
        };

        var validator = new ProdutoValidator();
        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Criar produto com caracteres especiais no codigo deve gerar erro.")]
    public void CriarProduto_ComCaractereEspecialNoCodigo_DeveFalhar()
    {
        var dto = new ProdutoCriarDto
        {
            TituloProduto = null,
            PrecoProduto = 10,
            EstoqueProduto = 5,
            CodigoProduto = "A@BC123@"
        };

        var validator = new ProdutoValidator();
        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Deve salvar um novo produto no banco de dados.")]
    public async Task DeveSalvarProdutoNoBanco()
    {
        using var context = DbContextFactory.Create();
        var repository = new ProdutoRepository(context);
        var produto = new Produto("Produto Teste", "Desc", 10, 5, "ABC123");

        await repository.AdicionarProdutoAsync(produto);

        var produtoSalvo = await context.Produtos.FirstOrDefaultAsync();

        Assert.NotNull(produtoSalvo);
    }

    [Fact(DisplayName = "Deve salvar um novo produto no banco de dados e encontrar o mesmo.")]
    public async Task DeveSalvarProdutoNoBancoEBuscar()
    {
        using var context = DbContextFactory.Create();
        var repository = new ProdutoRepository(context);
        var produto = new Produto("Produto Teste", "Desc", 10, 5, "ABC123");

        await repository.AdicionarProdutoAsync(produto);

        var produtoSalvo = await context.Produtos.FirstOrDefaultAsync();
        var produtoEncontrado = await repository.BuscarProdutoPorIdAsync(produtoSalvo!.ProdutoId);

        Assert.NotNull(produtoEncontrado);
        Assert.Equal(produtoSalvo.ProdutoId, produtoEncontrado!.ProdutoId);
    }
}
