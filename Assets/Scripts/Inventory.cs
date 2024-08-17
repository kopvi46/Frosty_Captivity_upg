using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    private static Dictionary<Item, int> inventoryList = new Dictionary<Item, int>();

    public static void AddItemToInventory(Item itemPicked)
    {
        Item sameItem = null;
        foreach (KeyValuePair<Item, int> item in inventoryList)
        {

            if (item.Key.GetItemSO().name == itemPicked.GetItemSO().name)
            {
                sameItem = item.Key;
                break;
            }
        }
        if (sameItem != null)
        {
            inventoryList[sameItem]++;
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
