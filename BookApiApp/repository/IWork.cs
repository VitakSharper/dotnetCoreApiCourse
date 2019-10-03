using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface IWork
    {
        Task<bool> Add<T>(T entity) where T : class;
        Task<bool> Delete<T>(T entity) where T : class;
        Task<bool> Save();
    }
}
