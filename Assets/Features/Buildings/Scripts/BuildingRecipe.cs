using System;
using System.Collections.Generic;
using Features.Items.Scripts;
using UnityEngine;

namespace Features.Buildings.Scripts
{
    [CreateAssetMenu]
    public class BuildingRecipe : ScriptableObjectByType
    {
        public List<RecipeData> recipeData;
        public GameObject building;
        public int requiredBuildingKernelSize;
    }
    
    [Serializable] 
    public class RecipeData
    {
        public BaseItem_SO requiredItem;
        public int requiredCount;

        public RecipeData(BaseItem_SO requiredItem, int requiredCount)
        {
            this.requiredItem = requiredItem;
            this.requiredCount = requiredCount;
        }

        public void ReduceRequiredCount()
        {
            requiredCount--;
        }
    }
}
