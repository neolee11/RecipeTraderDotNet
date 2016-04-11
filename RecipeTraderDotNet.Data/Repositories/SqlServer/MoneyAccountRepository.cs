using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.Core.Domain.User;

namespace RecipeTraderDotNet.Data.Repositories.SqlServer
{
    public class MoneyAccountRepository : IMoneyAccountRepository
    {
        public List<MoneyAccount> GetAll()
        {
            throw new NotImplementedException();
        }

        public MoneyAccount GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(MoneyAccount t)
        {
            throw new NotImplementedException();
        }

        public void Update(MoneyAccount t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public MoneyAccount GetUserMoneyAccount(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
