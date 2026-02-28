using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarProdutoAsync(Produto produto)
        {
            _context.Produtos.AddAsync(produto);

            await _context.SaveChangesAsync();
        }

        public async Task<Produto?> BuscarProdutoPorIdAsync(Guid produtoId)
        {
            return await _context.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == produtoId);
        }

        public async Task<IEnumerable<Produto>> BuscarProdutosAsync()
        {
            var produtos = await _context.Produtos
                            .AsNoTracking()
                            .ToListAsync();

            return produtos;
        }

        public async Task AtualizarProdutoAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarProdutoAsync(Guid produtoId)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == produtoId);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarListaAsync(List<Produto> produtos)
        {
            await _context.Produtos.AddRangeAsync(produtos);
            await _context.SaveChangesAsync();
        }
    }
}
