using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IUsable
{
    private float _torchBurningDuration = 10;
    private float _torchBurningTimer = 0;

    private void Update()
    {
        if (InventoryManager.Instance.LeftHandSlot.transform.childCount != 0
            && InventoryManager.Instance.LeftHandSlot.GetComponentInChildren<InventoryItem>().ItemSO is EquipmentSO equipment
            && equipment.equipmentType == EquipmentSO.EquipmentType.Torch)
        {
            _torchBurningTimer += Time.deltaTime;

            if (_torchBurningTimer > _torchBurningDuration)
            {
                Destroy(gameObject);

                foreach (Transform child in InventoryManager.Instance.LeftHandSlot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void UseItem(Player player)
    {
        
    }
}
