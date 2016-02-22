using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Data
{
    public class RecipeTraderModelContext : DbContext
    {
        public RecipeTraderModelContext()
            : base("RecipeTraderConnStr")
        {
            
        }
    }
}
