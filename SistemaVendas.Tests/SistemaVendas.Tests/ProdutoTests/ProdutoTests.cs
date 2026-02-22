using System.Runtime.ConstrainedExecution;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using Xunit;

public class ProdutoTests
{
    [Fact(DisplayName = "Gera exceção ao criar um produto com preço negativo.")]
    public void CriarProduto_ComPrecoNegativo_DeveLancarErro()
    {
        // Arrange
        var nome = "Produto Teste";
        var descricao = "Descrição do produto teste";
        var preco = -10.0m; // Preço negativo
        var estoque = 5;
        var codigo = "PROD123";

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => new Produto(nome, descricao, preco, estoque, codigo));
        Assert.Equal("Preço não pode ser negativo.", ex.Message);
    }

    [Fact(DisplayName = "Criar produto com título nulo não deve ser aceito.")]
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
}
