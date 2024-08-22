using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;

    [HideInInspector] public int amount = 1;

    public void Interact(Player player)
    {
        bool itemAddedSuccessfull = InventoryManager.Instance.AddInventoryItem(itemSO, this, amount);

        if (itemAddedSuccessfull)
        {
            Destroy(gameObject);
        }
    }
}
