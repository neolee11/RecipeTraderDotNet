using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Tests.Utilities
{
    public static class TestHelper
    {
        public static DateTime CreateTimeConstraintLowerBound(this DateTime currentTime)
        {
            return currentTime - TimeSpan.FromSeconds(1);
        }

        public static DateTime CreateTimeConstraintUpperBound(this DateTime currentTime)
        {
            return currentTime + TimeSpan.FromSeconds(1);
        }
    }
}
