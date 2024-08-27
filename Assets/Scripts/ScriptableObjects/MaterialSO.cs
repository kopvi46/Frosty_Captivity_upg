using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item/Material")]
public class MaterialSO : ItemSO
{
    public enum MaterialType
    {
        Stone,
        Log,
        Branch,
        Plank,
    }

    public MaterialType materialType;
    public int fireRestoraion;

    public override Enum GetSpecificItemType()
    {
        return materialType;
    }
}
