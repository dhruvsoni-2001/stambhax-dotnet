using System.Linq.Expressions;

namespace StambhaX.Api.Repositories
{
    public interface IRepository<T> where T : class
    {
        //get all records of type T from the database asynchronously
        Task<IEnumerable<T>> GetAllAsync();

        //get a record of type T by its id from the database asynchronously
        Task<T?> GetByIdAsync(Guid id);

        //Find records matching a specific condition (like where IsDeleted == false)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);


        //add a new record of type T to the database asynchronously
        Task AddAsync(T entity);

        //update an existing record of type T in the database synchronously
        void Update(T entity);

        //delete a record of type T from the database synchronously
        void Delete(T entity);

        //Save changes to the database asynchronously
        Task SaveChangesAsync();

    }

}
