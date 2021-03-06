﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public interface IPrivateRecipe
    {
        void Add(RecipeItem item);
        void Insert(RecipeItem item, int index);
        void Remove(RecipeItem item);
        void Clear();
    }
   
}
