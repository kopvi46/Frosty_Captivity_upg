using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Rock,
        ChoppedWood,
        Branch,
    }

    public ItemType itemType;
    public bool isStackable;
    public int maxStackAmount;
    public Transform prefab;
    public Sprite sprite;
}
