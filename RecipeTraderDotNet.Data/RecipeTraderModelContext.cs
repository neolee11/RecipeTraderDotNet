using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Data
{
    public class RecipeTraderModelContext : DbContext
    {
        public DbSet<PrivateRecipe> PrivateRecipes { get; set; }

        public RecipeTraderModelContext()   
            : base("RecipeTraderConnStr")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SalesModelContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<CoreModelContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CoreModelContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RecipeTraderModelContext>());
        }
    }

    //public class MyDropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<CoreModelContext>
    //{
    //    protected override void Seed(CoreModelContext context)
    //    {
    //        var department = new Department() { Name = "Computer Science", Budget = 1000000.00m, StartDate = new DateTime(1990, 1, 1) };
    //        context.Departments.Add(department);
    //        base.Seed(context);
    //    }
    //}
}
