using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Entity
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
