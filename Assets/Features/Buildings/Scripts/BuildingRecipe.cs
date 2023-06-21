using System;
using System.Linq;
using Features.Items.Scripts;
using UnityEngine;

namespace Features.Buildings.Scripts
{
    [CreateAssetMenu]
    public class BuildingRecipe : ScriptableObjectByType
    {
        [SerializeField] private RecipeData recipeData;

        public BaseItem_SO[,] GetRecipeKernel()
        {
            var baseItemKernel = new BaseItem_SO[RecipeData.KernelSize, RecipeData.KernelSize];
            
            for (int x = 0; x < RecipeData.KernelSize; x++)
            {
                for (int y = 0; y < RecipeData.KernelSize; y++)
                {
                    baseItemKernel[x, RecipeData.KernelSize - 1 - y] = recipeData.rows[y].row[x];
                }
            }

            return baseItemKernel;
        }
    }
    
    [Serializable] 
    public class RecipeData
    {
        public static int KernelSize = 3;
        
        [Serializable]
        public struct RowData
        {
            public BaseItem_SO[] row;
        }

        public RowData[] rows = new RowData[KernelSize];
    }
}
