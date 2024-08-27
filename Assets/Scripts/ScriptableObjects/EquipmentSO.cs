using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Equipment")]
public class EquipmentSO : ItemSO
{
    public enum EquipmentType
    {
        Torch,
        Spear,
        Axe,
    }

    public enum HandType
    {
        LeftHand,
        RightHand,
    }

    public EquipmentType equipmentType;
    public HandType handType;

    public override Enum GetSpecificItemType()
    {
        return equipmentType;
    }
}
