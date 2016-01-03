using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.RecipeTests
{
    public class RecipeTests
    {
        [Fact]
        public void BaseTest()
        {
            var a = 1;
            Assert.Equal(a, 1);

        }
    }
}
