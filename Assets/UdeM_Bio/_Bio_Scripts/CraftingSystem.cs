using static UnityEditor.Progress;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem: MonoBehaviour
{
    public Inventory playerInventory;
    public List<Recipe> recipes;

    public bool CraftNutrient(Recipe recipe)
    {
        foreach (Nutrient item in recipe.requiredItems)
        {
            if (!playerInventory.items.ContainsKey(item) || playerInventory.items[item] < 1) // Assuming each recipe needs one of each required item
            {
                return false; // Required item not found or not enough quantity
            }
        }

        // Remove used items
        foreach (Nutrient item in recipe.requiredItems)
        {
            playerInventory.RemoveNutrient(item, 1); // Assuming each recipe needs one of each required item
        }

        // Add the result item
        playerInventory.AddNutrient(recipe.resultItem, 1);

        return true;
    }
}
