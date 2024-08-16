using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    private static Dictionary<Item, int> inventoryList = new Dictionary<Item, int>();

    public static void AddItemToInventory(Item itemPicked)
    {
        bool hasSameItemAlready = false;
        foreach (KeyValuePair<Item, int> item in inventoryList)
        {
            if (item.Key == itemPicked)
            {
                hasSameItemAlready = true;
                break;
            }
        }
        if (hasSameItemAlready)
        {
            inventoryList[itemPicked]++;
        } else
        {
            inventoryList.Add(itemPicked, 1);
        }

        foreach (KeyValuePair<Item, int> item in inventoryList)
        {
            Debug.Log($"{item.Key.GetItemSO().name}: {item.Value}");
        }
    }
}
