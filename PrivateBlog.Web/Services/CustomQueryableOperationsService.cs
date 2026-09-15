using AutoMapper;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Abstractions;

namespace PrivateBlog.Web.Services
{
    public class CustomQueryableOperationsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomQueryableOperationsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<TDto>> CreateAsync<TDto, TEntity>(TDto dto) where TEntity : IId
        {
            try
            {
                TEntity entity = _mapper.Map<TEntity>(dto);

                Guid id = Guid.NewGuid();

                entity.Id = id;

                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return Response<TDto>.Success(dto, "Entidad creada con éxito");
            }
            catch(Exception ex)
            {
                return Response<TDto>.Failure(ex);
            }
        }
    }
}
