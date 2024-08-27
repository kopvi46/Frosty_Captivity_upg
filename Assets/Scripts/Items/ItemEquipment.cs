using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipment : MonoBehaviour
{
    public int _durability;
    private Item itemTransform;

    private void Start()
    {
        itemTransform = GetComponent<Item>();
        _durability = itemTransform.durability;

        itemTransform.OnDurabilityChanged += ItemTransform_OnDurabilityChanged;
    }

    private void ItemTransform_OnDurabilityChanged(object sender, System.EventArgs e)
    {
        _durability = itemTransform.durability;
    }

    private void Update()
    {
        if (_durability <= 0)
        {
            Destroy(gameObject);

            foreach (Transform child in InventoryManager.Instance.RightHandSlot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
