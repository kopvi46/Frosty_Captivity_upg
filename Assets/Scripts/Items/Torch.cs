using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private float _torchBurningDuration = 1;
    private float _torchBurningTimer = 0;
    public int _durability;

    private void Start()
    {
        _durability = transform.GetComponent<Item>().durability;
    }

    private void Update()
    {
        if (InventoryManager.Instance.LeftHandSlot.transform.childCount != 0
            && InventoryManager.Instance.LeftHandSlot.GetComponentInChildren<InventoryItem>().ItemSO is EquipmentSO equipment
            && equipment.equipmentType == EquipmentSO.EquipmentType.Torch)
        {
            _torchBurningTimer += Time.deltaTime;

            if (_torchBurningTimer > _torchBurningDuration)
            {
                _durability -= 5;

                InventoryEquipmentItem inventoryEquipmentItem = InventoryManager.Instance.LeftHandSlot.GetComponentInChildren<InventoryEquipmentItem>();

                if (inventoryEquipmentItem.ItemSO is EquipmentSO equipmentSO && equipmentSO.equipmentType == EquipmentSO.EquipmentType.Torch)
                {
                    inventoryEquipmentItem.durability = _durability;
                    inventoryEquipmentItem.RefreshDurability();
                }
                _torchBurningTimer = 0;
            }

            if (_durability <= 0)
            {
                Destroy(gameObject);

                foreach (Transform child in InventoryManager.Instance.LeftHandSlot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
