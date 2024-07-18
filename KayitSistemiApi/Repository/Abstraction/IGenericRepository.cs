namespace KayitSistemiApi.Repository.Abstraction
{
    public interface IGenericRepository<T> where T: class
    {
        void Save(T entity);
        void Delete(T entity);
        void Update(T entity);
        T FindById(int id);
        public int GenerateUniqueIntKey();
        ICollection<T> GetAll();
        IQueryable<T> GetQueryable();


    }
}
