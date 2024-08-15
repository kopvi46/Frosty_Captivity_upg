using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    public static Fireplace Instance;

    [SerializeField] private int fireplaceHealth;
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
}
