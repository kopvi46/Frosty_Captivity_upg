using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Rock,
        ChoppedWood,
        Branch,
        Torch,
    }
    public enum HandType
    {
        LeftHand,
        RightHand,
        None,
    }

    public ItemType itemType;
    public HandType handType;
    public bool isStackable;
    public int maxStackAmount;
    public Transform prefab;
    public Sprite sprite;
}
