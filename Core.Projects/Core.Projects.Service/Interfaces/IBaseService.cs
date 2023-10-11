using Common.Model.Search;

namespace Core.Projects.Service.Interfaces
{
    public interface IBaseService<T, TResponse, TBasicResponse, TSearchQuery>
    {
        Task<TResponse> Get(int id);

        Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery);

        Task<bool> Delete(int id);
    }
}
