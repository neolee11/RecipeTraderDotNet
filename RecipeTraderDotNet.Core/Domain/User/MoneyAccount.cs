using System.Collections.Generic;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.User
{
    public class MoneyAccount : BaseEntity
    {
        public string UserId { get; set; }
        public decimal Balance { get; set; }

        //public List<string> Transactions { get; set; } //todos: may implement this later

        public override string ToString()
        {
            return $"User : {UserId}\nBalance : {Balance}";
        }
    }
}