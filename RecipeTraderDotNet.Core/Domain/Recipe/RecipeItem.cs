using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public class RecipeItem : BaseEntity
    {
        public RecipeBase ParentRecipe { get; set; } //navigation property

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                ParentRecipe.TimeLastModified = DateTime.UtcNow;
            }
        }
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        public RecipeItemStatus Status { get; set; } = RecipeItemStatus.New;
        public DateTime TimeLastStatusChange { get; set; } = DateTime.UtcNow;
        

        public RecipeItem(string description, RecipeBase parentRecipe)
        {
            _description = description;
            ParentRecipe = parentRecipe;
        }

        public RecipeItem DeepCopyUsingSerialization(bool keepId = true, RecipeBase newParent = null)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    var newObj = (RecipeItem)formatter.Deserialize(stream);

                    if (!keepId)
                        newObj.Id = 0;

                    if (newParent != null)
                        newObj.ParentRecipe = newParent;

                    return newObj;
                }
                return null;
            }
        }

        public RecipeItem DeepCopy(bool keepId = true, RecipeBase newParent = null, bool setStatusNow = true)
        {
            var newObj = this.Copy();

            if (newObj == null) return null;

            if (setStatusNow)
            {
                newObj.Status = RecipeItemStatus.New;
                newObj.TimeLastStatusChange = DateTime.UtcNow;
            }

            if (!keepId)
                newObj.Id = 0;

            if (newParent != null)
                newObj.ParentRecipe = newParent;

            return newObj;
        }

        public void Finish()
        {
            this.Status = RecipeItemStatus.Done;
            this.TimeLastStatusChange = DateTime.UtcNow;
        }

        public void Reset()
        {
            this.Status = RecipeItemStatus.New;
            this.TimeLastStatusChange = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{Description} [{Id}]\tStatus: {Status}  Created on: {TimeCreated.ToLocalTime()} Last Status Change: {TimeLastStatusChange.ToLocalTime()}";
        }
    }
}
