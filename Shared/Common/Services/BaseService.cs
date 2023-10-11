using AutoMapper;
using Common.Interfaces;
using Common.Model.Enitites;
using Common.Model.Response;
using Common.Model.Search;
using DelegateDecompiler.EntityFrameworkCore;
using HashidsNet;
using Localization.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Services
{
    public abstract class BaseService<TContext, T, TResponse, TBasicResponse, TSearchQuery> : IBaseService<TContext, T, TResponse, TBasicResponse, TSearchQuery>
        where T : BaseEntity
        where TContext : BaseDbContext
        where TResponse : BaseResponse
        where TBasicResponse : BaseResponse
        where TSearchQuery : BaseSearchQuery
    {
        protected readonly TContext _ctx;
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<SharedResource> _stringLocalizer;
        protected readonly IHashids _hashids;

        public const int PageSize = 10;

        public BaseService(TContext ctx,
                           IMapper mapper,
                           IStringLocalizer<SharedResource> stringLocalizer,
                           IHashids hashids
                           )
        {
            _ctx = ctx;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _hashids = hashids;
        }

        public virtual async Task<TResponse> Get(int id)
        {
            T entity;
            entity = await GetFromDB(id);

            return _mapper.Map<TResponse>(entity);
        }

        public async Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery)
        {
            var pageSize = searchQuery.PageSize <= 0 ? PageSize : searchQuery.PageSize;
            var pageNumber = searchQuery.PageNumber <= 0 ? 1 : searchQuery.PageNumber;

            var querable = SearchQuery(GetQueryable(SearchIncludes()), searchQuery).DecompileAsync();

            var entities = await querable.Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            return new SearchResponse<TBasicResponse>
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = await querable.CountAsync(),
                Result = _mapper.Map<List<TBasicResponse>>(entities)
            };
        }

        public virtual async Task<bool> Delete(int id)
        {
            var result = false;
            var entity = await _ctx.Set<T>().FindAsync(id);

            if (entity != default)
            {
                _ctx.Remove(entity);

                await _ctx.SaveChangesAsync();

                result = true;
            }


            return result;
        }

        protected virtual string[] Includes()
        {
            return default;
        }

        protected virtual string[] SearchIncludes()
        {
            return default;
        }

        protected IQueryable<T> GetQueryable(string[] includes = default)
        {
            var queryable = _ctx.Set<T>().AsQueryable();

            if (includes == default)
                return queryable;

            foreach (var include in includes)
                queryable = queryable.Include(include);

            return queryable;
        }

        protected IQueryable<T> SearchQuery(IQueryable<T> queryable, TSearchQuery searchQuery)
        {
            queryable = SearchQueryInternal(queryable, searchQuery);

            queryable = queryable.OrderByDescending(x => x.Id);

            return queryable;
        }

        protected virtual IQueryable<T> SearchQueryInternal(IQueryable<T> queryable, TSearchQuery searchQuery)
        {
           return default;
        }

        private async Task<T> GetFromDB(int id)
        {
            var entity = await GetQueryable(Includes())
                               .DecompileAsync()
                               .FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        private static string GetRecordKey(string id)
        {
            return $"{typeof(T).Name}_{id}";
        }
    }
}
