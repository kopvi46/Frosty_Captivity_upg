using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    public void Interact()
    {
        Debug.Log($"Player picked {itemSO.name} !");

        Inventory.AddItemToInventory(this);

        Destroy(gameObject);
    }

    public ItemSO GetItemSO()
    {
        return itemSO;
    }
}
