using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class RecipeItem : BaseEntity
    {
        public string Description { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }
        public RecipeExecutionStatus Status { get; set; }
        public DateTime TimeLastStatusChange { get; set; }

        public RecipeItem()
        {
            Description = string.Empty;
            Status = RecipeExecutionStatus.New;

            var now = System.DateTime.UtcNow;
            TimeCreated = now;
            TimeLastModified = now;
            TimeLastStatusChange = now;

        }
    }
}
