using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem: MonoBehaviour
{
    public IngredientsContainer ingredientsContainer, productoBox;
    public GameObject productPrefab;

    public List<Recipe> recipes;

    public void QuickCraft(){
        List<Nutrient> ingredients = ingredientsContainer.GetIngredientsInContainer();
        List<Recipe> validRecipes = ReturnValidRecipes(ingredients);
        if (validRecipes.Count > 0)
        {
            CraftNutrient(validRecipes[0]);
        }
    
    }

    public bool CraftNutrient(Recipe recipe)
    {
        List<Nutrient> ingredients = ingredientsContainer.GetIngredientsInContainer();
        if (recipe.requiredItems.Count != ingredients.Count)
        {
            return false;
        }
        foreach (Nutrient item in recipe.requiredItems)
        {
            if (!ingredients.Contains(item))
            {
                return false;
            }
        }
        foreach (Transform child in ingredientsContainer.transform){
            Destroy(child.gameObject);
        }
        GameObject product = Instantiate(productPrefab, productoBox.transform);
        product.SetActive(true);
        product.GetComponent<Nutrient>().CopyNutrient(recipe.resultItem);
        product.GetComponent<Image>().sprite =recipe.resultItem.sprite;
        return true;
    }

    public List<Recipe> ReturnValidRecipes(List<Nutrient> ingredients){
        
        List<Recipe> validRecipes = new List<Recipe>();
        foreach (Recipe recipe in recipes)
        {
            bool valid = true;
            foreach (Nutrient item in recipe.requiredItems)
            {
                if (!ingredients.Contains(item))
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                validRecipes.Add(recipe);
            }
        }
        return validRecipes;
    }
}
