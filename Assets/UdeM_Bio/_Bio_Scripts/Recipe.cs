using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    public List<Nutrient> requiredItems;
    public Nutrient resultItem;

    public Recipe(List<Nutrient> requiredItems, Nutrient resultItem)
    {
        this.requiredItems = requiredItems;
        this.resultItem = resultItem;
    }
}
