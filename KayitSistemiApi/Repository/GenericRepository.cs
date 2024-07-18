using KayitSistemiApi.Data;
using KayitSistemiApi.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace KayitSistemiApi.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class

    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            SaveChanges();
        }

        public T FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public int GenerateUniqueIntKey()
        {
            return 0;
        }

        public ICollection<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public void Save(T entity)
        {
            _dbSet.Add(entity);
            SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            SaveChanges();
        }

        private void SaveChanges()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
