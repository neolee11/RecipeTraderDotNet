using System.Collections.Generic;
using RecipeTraderDotNet.Core.Domain.User;
using RecipeTraderDotNet.Data.Repositories.Memory;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Data.Tests.RepositoryTests.MemoryRepositoryTests
{
    public class MoneyAccountRepositoryTests
    {
        private List<MoneyAccount> GetCurrentMoneyAccountSystemState()
        {
            var state = new List<MoneyAccount>();
            state.Add(new MoneyAccount
            {
                Id = 1,
                UserId = "user1",
                Balance = 100
            });

            state.Add(new MoneyAccount
            {
                Id = 2,
                UserId = "user2",
                Balance = 200
            });

            state.Add(new MoneyAccount
            {
                Id = 3,
                UserId = "user3",
                Balance = 300
            });

            return state;
        }

        [Fact]
        public void GetAllShouldGetAllOfMoneyAccounts()
        {
            var state = GetCurrentMoneyAccountSystemState();
            var sut = new MoneyAccountRepository(state);

            var results = sut.GetAll();

            results.Count.ShouldEqual(state.Count);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].UserId.ShouldEqual(state[i].UserId);
                results[i].Balance.ShouldEqual(state[i].Balance);
            }
        }

        [Fact]
        public void GetByIdShouldWork()
        {
            var state = GetCurrentMoneyAccountSystemState();
            var sut = new MoneyAccountRepository(state);

            var resultNull = sut.GetById(100);
            resultNull.ShouldBeNull();

            var result = sut.GetById(2);
            result.ShouldNotBeNull();
            result.UserId.ShouldEqual("user2");
            result.Balance.ShouldEqual(200);
        }

        [Fact]
        public void InsertShouldWork()
        {
            var sut = new MoneyAccountRepository(GetCurrentMoneyAccountSystemState());
            var newAccount = new MoneyAccount {Id = 6, UserId = "user6", Balance = 600};

            sut.Insert(newAccount);

            var results = sut.GetAll();
            results.Count.ShouldEqual(4);
            results[results.Count - 1].UserId.ShouldEqual("user6");
            results[results.Count - 1].Balance.ShouldEqual(600);
        }

        [Fact]
        public void UpdateShouldWork()
        {
            var sut = new MoneyAccountRepository(GetCurrentMoneyAccountSystemState());
            var newBalance = 150;
            var updatedRecord = new MoneyAccount
            {
                Id = 1,
                UserId = "user1",
                Balance = newBalance
            };

            sut.Update(updatedRecord);

            var result = sut.GetById(1);
            result.Balance.ShouldEqual(newBalance);
        }

        [Fact]
        public void DeleteShouldWork()
        {
            var sut = new MoneyAccountRepository(GetCurrentMoneyAccountSystemState());
            var idToDelete = 1;
            sut.Delete(idToDelete);

            var results = sut.GetAll();
            results.Count.ShouldEqual(2);

            var account1 = sut.GetById(idToDelete);
            account1.ShouldBeNull();
        }

        [Fact]
        public void GetUserMoneyAccountShouldWork()
        {
            var sut = new MoneyAccountRepository(GetCurrentMoneyAccountSystemState());
            var userId = "user1";

            var result = sut.GetUserMoneyAccount(userId);

            result.ShouldNotBeNull();
            result.UserId.ShouldEqual("user1");
            result.Balance.ShouldEqual(100);

            var fakeUserId = "user100";
            var result1 = sut.GetUserMoneyAccount(fakeUserId);
            result1.ShouldBeNull();
        }
    }
}
