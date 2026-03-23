using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;
    Task<int> SaveAsync(CancellationToken cancellationToken);
    Task Rollback();
}
