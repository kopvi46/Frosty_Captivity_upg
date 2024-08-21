using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObtainableResourseSO : ScriptableObject
{
    public Transform prefab;
    public ItemSO itemSO;
    public float obtainProgressMax;
    public float spawnItemAmount;
    public string objectName;
}
