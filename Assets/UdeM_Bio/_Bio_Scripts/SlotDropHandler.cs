using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class SlotDropHandler : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public CraftingSystem craftingSystem;
    public bool isInputSlot;
    public bool isOutputSlot;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = GetComponent<RectTransform>();
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
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
            if (isInputSlot)
            {
                inventory.AddNutrient(item, 1); // Assuming item is being added to the crafting input
                Debug.Log("Added " + item.NutrientName + " to inventory");
                CheckCraftingRecipes();
            }
            else if (isOutputSlot)
            {
                inventory.RemoveNutrient(item, 1); // Move item from output to ingredients
                Debug.Log("Removed " + item.NutrientName + " from inventory");
            }
        }
    }

    private void CheckCraftingRecipes()
    {
        // Here, iterate over your recipes to check if the items in the input match any recipe.
        // If a match is found, craft the item (remove inputs and add output).
        // This is a simplified approach; you'll need to implement the logic based on your recipe structure.
    }
}
