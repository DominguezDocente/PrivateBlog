using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PrivateBlog.Web.Services
{
    public class CustomBaseService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomBaseService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<TDTO>>> GetListAsync<TEntity, TDTO>() where TEntity : class
        {
            try
            {
                List<TEntity> list = await _context.Set<TEntity>()
                                                   .AsNoTracking()
                                                   .ToListAsync();

                List<TDTO> listDTO = _mapper.Map<List<TDTO>>(list);

                return ResponseHelper<List<TDTO>>.MakeResponseSuccess(listDTO);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<TDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<TDTO>>> GetPaginationAsync<TEntity, TDTO>(PaginationRequest request, IQueryable<TEntity> queryable = null) 
            where TEntity : class
            where TDTO : class
        {
            try
            {
                if (queryable is null)
                {
                    queryable = _context.Set<TEntity>();
                }

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    // Obtener las propiedades de TEntity con el atributo [FilterableAsString]
                    List<string> filterableProperties = typeof(TEntity).GetProperties()
                                    .Where(p => p.GetCustomAttribute<FilterableAsStringAttribute>() != null && p.PropertyType == typeof(string))
                                    .Select(p => p.Name)
                                    .ToList();

                    if (filterableProperties.Any())
                    {
                        // Construir dinámicamente la condición de búsqueda con LINQ dinámico
                        string predicate = string.Join(" || ", filterableProperties.Select(p => $"{p}.Contains(@0)"));

                        queryable = queryable.Where(predicate, request.Filter);
                    }
                }

                PagedList<TEntity> list = await PagedList<TEntity>.ToPagedListAsync(queryable, request);

                PaginationResponse<TDTO> result = new PaginationResponse<TDTO>
                {
                    List = _mapper.Map<PagedList<TDTO>>(list),
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<TDTO>>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<TDTO>>.MakeResponseFail(ex);
            }
        }

    }
}
