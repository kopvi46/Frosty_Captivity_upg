using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _itemSO;

    [HideInInspector] public int amount;
    [HideInInspector] public int durability;

    public ItemSO ItemSO
    {
        get { return _itemSO; }
        private set { _itemSO = value; }
    }

    public void Interact(Player player)
    {
        bool itemAddedSuccessfull = InventoryManager.Instance.AddInventoryItem(_itemSO, this, amount, durability);

        if (itemAddedSuccessfull)
        {
            Destroy(gameObject);
        }
    }
}
