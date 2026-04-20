namespace PrivateBlog.Application.Contracts.Pagination
{
    public sealed class PaginationResponse<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static PaginationResponse<T> Create(
            IReadOnlyList<T> items,
            int totalCount,
            PaginationRequest normalizedPagination)
        {
            PaginationRequest p = normalizedPagination.Normalized();
            return new PaginationResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = p.PageNumber,
                PageSize = p.PageSize,
            };
        }
    }
}
