using eMart.Models;

namespace eMart.Repository.Base
{
    public interface IRepository<T> where T :class 
    {
        public T? Find(int id);
        public T? selectone(Func<T, bool> predicate);
        public IEnumerable<T> FindAll();

        public T selectLast(Func<T, bool> predicate);
        public IEnumerable<T> FindAll(params string[] eagers);

      
        public Task<T> FindAsync(int id);

        public Task<IEnumerable<T>> FindAllAsync();

        public Task<IEnumerable<T>> FindAllAsync(params string[] eagers);

        public void Addone(T myItem);
        public void UpdateOne(T myItem);
        public void DeleteOne(T myItem);

        public void AddMany(IEnumerable<T> myItem);
        public void UpdateMany(IEnumerable<T> myItem);
        public void DeleteMany(IEnumerable<T> myItem);
   
    }
}
