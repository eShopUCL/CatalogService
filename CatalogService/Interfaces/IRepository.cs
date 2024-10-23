using Ardalis.Specification;
namespace CatalogService.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }

}
