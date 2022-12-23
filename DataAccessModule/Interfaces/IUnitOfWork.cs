using System.Data.Entity;
using System.Linq;

namespace DataAccessModule.Interfaces
{
    public interface IUnitOfWork
    {
        DbSet<TEntity> Repository<TEntity>() where TEntity : class;

        int SaveChanges();
    }
}
