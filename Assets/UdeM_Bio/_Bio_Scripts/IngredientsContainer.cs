using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IngredientsContainer : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public CraftingSystem craftingSystem;
    public bool isCraftingTable;
    public bool isProductSlot;
    public bool isInventory;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = GetComponent<RectTransform>();
        DraggableNutrient itemDragHandler = eventData.pointerDrag.GetComponent<DraggableNutrient>();
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            Debug.Log("Dropped outside inventory");
            itemDragHandler.ReturnToOriginalParent();
            return;
        }

        if (itemDragHandler != null)
        {
            itemDragHandler.SetParent(transform);
            Nutrient item = itemDragHandler.GetComponent<Nutrient>();
            if (isInventory)
            {
                inventory.AddNutrient(item, 1); // Assuming item is being added to the crafting input
                Debug.Log("Added " + item.NutrientName + " to inventory");
                CheckCraftingRecipes();
            }
            else if (isCraftingTable)
            {
                inventory.RemoveNutrient(item, 1); // Move item from inventory to crafting input
                Debug.Log("Removed " + item.NutrientName + " from inventory");
                CheckCraftingRecipes();
            }
            else if (isProductSlot)
            {
                Debug.Log("This is the Product slot");
            }
        }
    }

    public void PrintContainerContent()
    {
        List<Nutrient> items = GetIngredientsInContainer();
        foreach (Nutrient item in items)
        {
            Debug.Log(item.NutrientName);
        }
    }

    public List<Nutrient> GetIngredientsInContainer(){
        List<Nutrient> items = new List<Nutrient>();
        foreach (Transform child in transform)
        {
            Nutrient item = child.GetComponent<Nutrient>();
            if (item != null)
            {
                items.Add(item);
            }
        }
        return items;
    }


    private void CheckCraftingRecipes()
    {
        // Here, iterate over your recipes to check if the items in the input match any recipe.
        // If a match is found, craft the item (remove inputs and add output).
        // This is a simplified approach; you'll need to implement the logic based on your recipe structure.
    }
}
