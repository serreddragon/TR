using AutoMapper;
using Common.Model;
using Common.Model.Response;
using Common.Model.Search;
using Core.Projects.DAL;
using Core.Projects.Service.Interfaces;

namespace Core.Projects.Service.Services
{
    public abstract class BaseService<T, TResponse, TBasicResponse, TSearchQuery> : IBaseService<T, TResponse, TBasicResponse, TSearchQuery>
        where T : BaseEntity
        where TResponse : BaseResponse
        where TBasicResponse : BaseResponse
        where TSearchQuery : BaseSearchQuery
    {
        protected readonly ProjectsDbContext _ctx;
        protected readonly IMapper _mapper;

        public const int PageSize = 10;

        public BaseService(ProjectsDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public virtual async Task<TResponse> Get(int id)
        {

            throw new NotImplementedException();
        }

        public Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
