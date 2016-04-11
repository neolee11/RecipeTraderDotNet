using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.Core.Domain.User;

namespace RecipeTraderDotNet.Data.Repositories.Memory
{
    public class MoneyAccountRepository : IMoneyAccountRepository
    {
        private List<MoneyAccount> _currentMoneyAccountState;

        public MoneyAccountRepository(List<MoneyAccount> currentMoneyAccountState)
        {
            _currentMoneyAccountState = currentMoneyAccountState;
        }

        public List<MoneyAccount> GetAll()
        {
            return _currentMoneyAccountState;
        }

        public MoneyAccount GetById(int id)
        {
            return _currentMoneyAccountState.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(MoneyAccount t)
        {
            _currentMoneyAccountState.Add(t);
        }

        public void Update(MoneyAccount t)
        {
            var existing = _currentMoneyAccountState.SingleOrDefault(a => a.Id == t.Id);
            if (existing != null)
            {
                existing.UserId = t.UserId;
                existing.Balance = t.Balance;
            }
        }

        public void Delete(int id)
        {
            var existing = _currentMoneyAccountState.SingleOrDefault(a => a.Id == id);
            if (existing != null) _currentMoneyAccountState.Remove(existing);
        }

        public MoneyAccount GetUserMoneyAccount(string userId)
        {
            return _currentMoneyAccountState.SingleOrDefault(a => a.UserId == userId);
        }
    }
}
