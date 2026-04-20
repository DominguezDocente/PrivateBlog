namespace PrivateBlog.Application.Contracts.Pagination
{
    /// <summary>
    /// Entrada común para listados paginados (reutilizar en otros queries).
    /// </summary>
    public sealed class PaginationRequest
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 50;

        public int PageNumber { get; }

        public int PageSize { get; }

        public PaginationRequest()
            : this(1, DefaultPageSize)
        {
        }

        public PaginationRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;

            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }
            else if (pageSize < 1)
            {
                pageSize = DefaultPageSize;
            }

            PageSize = pageSize;
        }

        /// <summary>
        /// Devuelve una petición con valores seguros para consultas (reaplica reglas del constructor).
        /// </summary>
        public PaginationRequest Normalized()
        {
            return new PaginationRequest(PageNumber, PageSize);
        }
    }
}
