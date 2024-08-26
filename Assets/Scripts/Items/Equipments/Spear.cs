using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour, IUsable
{
    private int _durability;

    private void Start()
    {
        _durability = transform.GetComponent<Item>().durability;
    }

    public void UseItem(Player player)
    {
        throw new System.NotImplementedException();
    }
}
