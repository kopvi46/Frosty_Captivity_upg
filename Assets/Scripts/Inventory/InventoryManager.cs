using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }

    [SerializeField] private InventorySlot[] inventorySlotArray;
    [SerializeField] private GameObject inventoryItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public bool AddItem(ItemSO itemSO)
    {
        //Check if picked item is stakable and is there any slot that can store one more same item
        if (itemSO.isStackable)
        {
            for (int i = 0; i < inventorySlotArray.Length; i++)
            {
                InventorySlot inventorySlot = inventorySlotArray[i];
                InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.ItemSO == itemSO && itemInSlot.amount < itemInSlot.ItemSO.maxStackAmount)
                {
                    itemInSlot.amount++;
                    itemInSlot.RefreshAmount(); 
                    return true;
                }
            }
        }
        //Find empy slot for placing picked item
        for (int i = 0; i < inventorySlotArray.Length; i++)
        {
            InventorySlot inventorySlot = inventorySlotArray[i];
            InventoryItem itemInSlot = inventorySlot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(itemSO, inventorySlot);
                return true;
            }
        }
        return false;
    }

    private void SpawnNewItem(ItemSO itemSO, InventorySlot inventorySlot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(itemSO);
    }
}
