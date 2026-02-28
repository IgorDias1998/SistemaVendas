namespace SistemaVendas.Application.Interfaces
{
    public interface IProdutoImportService
    {
        Task ImportarAsync(Stream stream);
    }
}
