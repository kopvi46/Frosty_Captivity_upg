using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CraftManager;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }

    [SerializeField] private CraftRecipe[] craftRecipeArray;

    [System.Serializable]
    public struct CraftRecipe
    {
        public CraftRecipeSO craftRecipeSO;
        public CraftItem craftItemObject;
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

    private void CheckAvailableRecipe()
    {
        foreach (CraftRecipe craftRecipe in craftRecipeArray)
        {
            foreach (CraftRecipeSO.Ingredient ingredient in craftRecipe.craftRecipeSO.ingredientsList)
            {
                InventorySlot[] inventorySlotArray = InventoryManager.Instance.GetInventorySlotArray();
                for (int i = 0; i < inventorySlotArray.Length; i++)
                {
                    if (ingredient.requiredItemSO.itemType == inventorySlotArray[i].GetComponentInChildren<InventoryItem>().ItemSO.itemType)
                    {

                    }
                }
            }
        }

        //bool canCraftRecipe = true;

        //foreach (CraftRecipeSO.Ingredient ingredient in craftRecipeSO.ingredientsList)
        //{
        //    bool ingredientFound = false;
        //    foreach (Item item in inventory.GetItemList())
        //    {
        //        if (item.GetItemSO().itemType == ingredient.requiredItemSO.itemType && item.GetItemAmount() >= ingredient.requiredAmount)
        //        {
        //            ingredientFound = true;
        //            break;
        //        }
        //    }

        //    // якщо хоча б один ≥нгред≥Їнт не знайдено або в недостатн≥й к≥лькост≥
        //    if (!ingredientFound)
        //    {
        //        canCraftRecipe = false;
        //        break;
        //    }
        //}

        //if (canCraftRecipe)
        //{
        //    // ¬иконати крафт
        //}
    }
}
