using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }

    [SerializeField] private CraftRecipe[] _craftRecipeArray;

    [System.Serializable]
    public struct CraftRecipe
    {
        public CraftRecipeSO craftRecipeSO;
        public CraftItemButton craftItemButton;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += InventoryManager_OnInventoryChanged;
    }

    private void InventoryManager_OnInventoryChanged(object sender, System.EventArgs e)
    {
        CheckAvailableRecipe();
    }

    public void CheckAvailableRecipe()
    {
        foreach (CraftRecipe craftRecipe in _craftRecipeArray)
        {
            bool canCraftRecipe = true;

            foreach (CraftRecipeSO.Ingredient ingredient in craftRecipe.craftRecipeSO.ingredientsList)
            {
                int availableAmountOfRequiredIngredient = 0;
                Debug.Log(availableAmountOfRequiredIngredient);

                foreach (InventorySlot inventorySlot in InventoryManager.Instance.GetInventorySlotArray())
                {
                    InventoryItem inventoryItem = inventorySlot.GetComponentInChildren<InventoryItem>();

                    if (inventoryItem != null && inventoryItem.ItemSO.itemType == ingredient.requiredItemSO.itemType)
                    {
                        availableAmountOfRequiredIngredient += inventoryItem.amount;
                    }
                }
                Debug.Log(availableAmountOfRequiredIngredient);

                if (availableAmountOfRequiredIngredient < ingredient.requiredAmount)
                {
                    canCraftRecipe = false;
                    break;
                }
            }

            craftRecipe.craftItemButton.canCraftRecipe = canCraftRecipe;
            craftRecipe.craftItemButton.canvasGroup.alpha = canCraftRecipe ? 1f : .5f;
        }
    }
}
