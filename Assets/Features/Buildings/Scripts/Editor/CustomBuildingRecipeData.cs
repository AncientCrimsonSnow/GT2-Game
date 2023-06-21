using Features.Buildings.Scripts;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RecipeData))]
public class CustomBuildingRecipeData : PropertyDrawer 
{
    
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        
        Rect newPosition = position;
        newPosition.y += 18f;
        SerializedProperty rows = property.FindPropertyRelative("rows");
        rows.arraySize = 3;
        
        for(int i=0; i < RecipeData.KernelSize; i++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("row");
            newPosition.height = 20;

            if (row.arraySize != RecipeData.KernelSize)
                row.arraySize = RecipeData.KernelSize;

            newPosition.width = 70;

            for(int j=0; j < RecipeData.KernelSize; j++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
                newPosition.x += newPosition.width;
            }

            newPosition.x = position.x;
            newPosition.y += 20;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20 * 12;
    }
}
