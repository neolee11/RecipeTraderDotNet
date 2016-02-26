using System.Collections.Generic;

namespace RecipeTraderDotNet.Core.Domain.Repositories
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        T GetById(int id);
         
        void Insert(T t);

        void Update(T t);

        void Delete(int id);
    }
}
