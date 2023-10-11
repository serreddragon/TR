using Common.Model.Search;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IBaseService<TContext, T, TResponse, TBasicResponse, TSearchQuery>
    {
        Task<TResponse> Get(int id);
        Task<SearchResponse<TBasicResponse>> Search(TSearchQuery searchQuery);
        Task<bool> Delete(int id);
    }
}
