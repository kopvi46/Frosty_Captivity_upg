using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Torch,
        ChoppedWood,
        Branch,
    }

    public string ObjectName;
    public ItemType itemType;
    public bool isStackable;
    public Transform prefab;
    public Sprite sprite;
    //public int amount = 1;
}
