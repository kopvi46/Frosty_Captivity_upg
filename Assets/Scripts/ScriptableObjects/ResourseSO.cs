using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ResourseSO : ScriptableObject
{
    public enum ItemType
    {
        Rock,
        ChoppedWood,
        Branch,
    }

    public ItemType itemType;
    public bool isStackable;
    public Transform prefab;
    public Sprite sprite;
}
