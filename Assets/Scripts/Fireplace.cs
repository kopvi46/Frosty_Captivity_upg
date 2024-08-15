using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    public static Fireplace Instance;

    [SerializeField] private int fireplaceHealth;

    private bool isPlayerTriggered = false;

    public int FireplaceHealth 
    { 
        get 
        {  
            return fireplaceHealth; 
        } 
        set 
        { 
            if (value < 0)
            {
                fireplaceHealth = 0;
            } else
            {
                fireplaceHealth = value;
            }
        } 
    }

    private void Awake()
    {
        Instance = this;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<Player>(out Player player))
    //    {
    //        Debug.Log("Player near fireplace!");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            isPlayerTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            isPlayerTriggered = false;
        }
    }

    public bool GetIsPlayerTriggered()
    {
        return isPlayerTriggered;
    }
}
