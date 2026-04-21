using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Pagination
{
    public class PaginationResponse<T>
    {
        public required List<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public static PaginationResponse<T> Create(List<T> items, int totalCount, PaginationRequest request)
        {
            return new PaginationResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
