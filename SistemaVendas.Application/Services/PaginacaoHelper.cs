using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Services
{
    internal static class PaginacaoHelper
    {
        public static PagedResultDto<T> AplicarPaginacao<T>(IEnumerable<T> source, PagedQueryDto query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize < 1 ? 20 : Math.Min(query.PageSize, 100);

            var totalItems = source.Count();
            var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResultDto<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}
