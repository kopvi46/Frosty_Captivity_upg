using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ResourceSO : ScriptableObject
{
    public Transform prefab;
    public ItemSO itemSO;
    public float obtainProgressMax;
    public string objectName;
}
