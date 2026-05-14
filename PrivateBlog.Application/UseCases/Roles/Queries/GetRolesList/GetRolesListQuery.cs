using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList
{
    public sealed class GetRolesListQuery : IRequest<PaginationResponse<RoleListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Normalized();
        public string? NameFilter { get; set; }
    }
}
