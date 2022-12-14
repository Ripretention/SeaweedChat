using System.Threading.Tasks;

namespace SeaweedChat.Domain.Aggregates;
public interface IRepository<T> 
{
    Task<T?> Get(Guid id);
    Task<Boolean> Remove(T obj);
    Task<Boolean> Save(T obj);
}