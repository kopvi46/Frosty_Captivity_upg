using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    //[SerializeField] private TextMeshProUGUI amountVisual;
    private int amount = 1;


    public void Interact(Player player, Inventory inventory)
    {
        inventory.AddItemToInventory(this);

        Destroy(gameObject);
    }

    public void SetItemAmount(int newAmount)
    {
        amount = newAmount;
    }
    //public void SetItemAmountVisual()
    //{
    //    if (amount > 1)
    //    {
    //        amountVisual.SetText(amount.ToString());
    //    } else
    //    {
    //        amountVisual.SetText("");
    //    }
    //}

    public static Item DropItem(Item item)
    {
        float spawnRadius = Random.Range(3f, 4f);

        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0;

        Vector3 spawnPosition = Player.Instance.transform.position + randomOffset;

        float randomRotation = Random.Range(0f, 360f);
        Quaternion spawnRotation = Quaternion.Euler(0, randomRotation, 0);

        Transform itemTransform = Instantiate(item.itemSO.prefab, spawnPosition, spawnRotation);

        Item newItem = itemTransform.GetComponent<Item>();
        newItem.SetItemAmount(1);
        //newItem.SetItemAmountVisual();

        return newItem;
    }

    public ItemSO GetItemSO()
    {
        return itemSO;
    }

    public int GetItemAmount()
    {
        return amount;
    }

    public bool IsStakable()
    {
        return itemSO.isStackable;
    }
}
