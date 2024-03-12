using static UnityEditor.Progress;
using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    public Dictionary<Nutrient, int> items = new Dictionary<Nutrient, int>();

    public void AddNutrient(Nutrient item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        }
        else
        {
            items.Add(item, quantity);
        }
    }

    public bool RemoveNutrient(Nutrient item, int quantity)
    {
        if (items.ContainsKey(item) && items[item] >= quantity)
        {
            items[item] -= quantity;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
            return true;
        }
        return false;
    }
}
