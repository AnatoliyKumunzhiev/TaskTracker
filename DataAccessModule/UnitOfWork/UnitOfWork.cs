using System;
using System.Data.Entity;
using DataAccessModule.Interfaces;

namespace DataAccessModule.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public readonly AppDbContext Context;

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> Repository<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
