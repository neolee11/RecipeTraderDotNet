using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class RecipeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "New Item";
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public RecipeStatus Status { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }
        public DateTime TimeLastStatusChange { get; set; }

        public RecipeItem()
        {
            var now = System.DateTime.UtcNow;
            Status = RecipeStatus.New;

            TimeCreated = now;
            TimeLastModified = now;
            TimeLastStatusChange = now;

        }
    }
}
