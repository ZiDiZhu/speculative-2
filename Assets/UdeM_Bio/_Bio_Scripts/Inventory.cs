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

    public bool HasNutrient(Nutrient item, int quantity)
    {
        if (items.ContainsKey(item) && items[item] >= quantity)
        {
            return true;
        }
        return false;
    }

    [ContextMenu("Print Inventory Content")]
    public void PrintInventoryContent(){
        
        foreach (KeyValuePair<Nutrient, int> item in items)
        {
            Debug.Log(item.Key.NutrientName + " : " + item.Value);
        }
    }
}
