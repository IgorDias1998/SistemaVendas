using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface ILogMudancaRotaRepository
    {
        Task AdicionarAsync(LogMudancaRota log);
        Task<IEnumerable<LogMudancaRota>> BuscarPorRotaAsync(Guid rotaId);
    }
}
