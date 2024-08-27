using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Obtain Resource")]
public class ObtainableResourseSO : ScriptableObject
{
    public Transform prefab;
    public ItemSO itemSO;
    public EquipmentSO equipmentToObtain;
    public float obtainProgressMax;
    public float spawnItemAmount;
    public string objectName;
}
