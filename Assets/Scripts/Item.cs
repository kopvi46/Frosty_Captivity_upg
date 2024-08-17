using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemObjectSO;

    public void Interact(Player player, Inventory inventory)
    {
        Debug.Log($"Player picked {itemObjectSO.name} !");

        inventory.AddItemToInventory(this);

        Destroy(gameObject);
    }

    //public Sprite GetSprite()
    //{
    //    return itemObjectSO.sprite;
    //}

    public ItemSO GetItemSO()
    {
        return itemObjectSO;
    }

    public bool IsStakable()
    {
        return itemObjectSO.isStackable;
    }
}
