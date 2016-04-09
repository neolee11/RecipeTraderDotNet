using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.User;

namespace RecipeTraderDotNet.Core.Domain.Repositories
{
    public interface IMoneyAccountRepository : IRepository<MoneyAccount>
    {
        MoneyAccount GetUserMoneyAccount(string userId);
    }
}
