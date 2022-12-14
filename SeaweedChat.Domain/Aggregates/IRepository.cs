using System.Threading.Tasks;

namespace SeaweedChat.Domain.Aggregates;
public interface IRepository<T> 
{
    Task<T?> Get(Guid id);
    Task<bool> Remove(T obj);
    Task<T> Add(T obj);
    Task<bool> Update();
}