using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public abstract class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Material,
        Equipment,
    }

    public ItemType itemType;
    public Item item;
    public bool isStackable;
    public int maxStackAmount;
    public Transform prefab;
    public Sprite sprite;

    public abstract Enum GetSpecificItemType();

    public bool IsSameItemType(ItemSO otherItemSO)
    {
        return GetSpecificItemType().Equals(otherItemSO.GetSpecificItemType());
    }
}
