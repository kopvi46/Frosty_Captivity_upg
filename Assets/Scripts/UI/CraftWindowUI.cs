using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CraftWindowUI : MonoBehaviour
{
    [SerializeField] public List<CraftRecipeSO> craftRecipeSOList;

    private Inventory inventory;
    private Transform recipeSlotContainer;
    private Transform recipeSlotTemplate;
    private const string ITEM_SLOT_CONTAINER = "RecipeSlotContainer";
    private const string ITEM_SLOT_TEMPLATE = "RecipeSlotTemplate";
    private const string IMAGE = "Image";

    private void Awake()
    {
        recipeSlotContainer = transform.Find(ITEM_SLOT_CONTAINER);
        recipeSlotTemplate = recipeSlotContainer.Find(ITEM_SLOT_TEMPLATE);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        //inventory.AddItemToInventory(craftRecipeSOList[0].craftedItem);

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        CheckAvailableRecipe();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        CheckAvailableRecipe();
    }

    private void CheckAvailableRecipe()
    {
        foreach (Transform child in recipeSlotContainer)
        {
            if (child == recipeSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (CraftRecipeSO craftRecipeSO in craftRecipeSOList)
        {
            bool canCraftRecipe = true;
            foreach (CraftRecipeSO.Ingredient ingredient in craftRecipeSO.ingredientsList)
            {
                bool ingredientFound = false;
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.GetItemSO().itemType == ingredient.requiredItemSO.itemType && item.GetItemAmount() >= ingredient.requiredAmount)
                    {
                        ingredientFound = true;
                        break;
                    }
                }

                if (!ingredientFound)
                {
                    canCraftRecipe = false;
                    break;
                }
            }

            if (canCraftRecipe)
            {
                RectTransform itemSlotRectTransform = Instantiate(recipeSlotTemplate, recipeSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);

                InventoryClickHandler clickHandler = itemSlotRectTransform.GetComponent<InventoryClickHandler>();

                if (clickHandler != null)
                {
                    clickHandler = itemSlotRectTransform.gameObject.AddComponent<InventoryClickHandler>();
                }

                clickHandler.OnLeftClick += (sender, e) =>
                {
                    foreach (CraftRecipeSO.Ingredient ingredient in craftRecipeSO.ingredientsList)
                    {
                        for (int i = 0; i < ingredient.requiredAmount; i++)
                        {
                            inventory.RemoveItem(ingredient.requiredItem);
                        }
                    }
                    inventory.AddItemToInventory(craftRecipeSO.craftedItem);
                    CheckAvailableRecipe();
                };

                Image image = itemSlotRectTransform.Find(IMAGE).GetComponent<Image>();
                image.sprite = craftRecipeSO.craftedItemSO.sprite;
            }
        }
    }
}
