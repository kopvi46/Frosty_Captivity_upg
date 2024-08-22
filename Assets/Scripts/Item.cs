using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;

    public void Interact(Player player)
    {
        bool itemAddedSuccessfull = InventoryManager.Instance.AddItem(itemSO);

        if (itemAddedSuccessfull)
        {
            Destroy(gameObject);
        }
    }
}
