using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    
    private Action<Item> UseItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        itemList = new List<Item>();
        this.UseItemAction = useItemAction;
    }

    public void AddItemToInventory(Item item)
    {
        if (item.IsStakable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.GetItemSO().itemType == item.GetItemSO().itemType)
                {
                    //inventoryItem.SetItemAmount(inventoryItem.GetItemAmount() + 1);
                    inventoryItem.SetItemAmount(inventoryItem.GetItemAmount() + item.GetItemAmount());
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        } else
        {
            itemList.Add(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStakable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.GetItemSO().itemType == item.GetItemSO().itemType)
                {
                    inventoryItem.SetItemAmount(inventoryItem.GetItemAmount() - item.GetItemAmount());
                    itemInInventory = inventoryItem;
                }
            }
            if (!itemInInventory && itemInInventory.GetItemAmount() <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        } else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        UseItemAction(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
