using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour, IHasHealth, IInteractable
{
    public static Fireplace Instance;

    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private int fireplaceHealth = 100;
    private float fireplaceHealtChangeDelay = 3f;
    private float fireplaceHealtChangeTimer = 0f;

    public int FireplaceHealth 
    { 
        get 
        {  
            return fireplaceHealth; 
        } 
        set 
        {
            int fireplaceMinHealth = 0;
            int fireplaceMaxHealth = 100;
            if (value < fireplaceMinHealth)
            {
                fireplaceHealth = fireplaceMinHealth;
            } else if (value > fireplaceMaxHealth)
            {
                fireplaceHealth = fireplaceMaxHealth;
            } else
            {
                fireplaceHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)FireplaceHealth / fireplaceMaxHealth
            });
        } 
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        fireplaceHealtChangeTimer -= Time.deltaTime;

        if (fireplaceHealtChangeTimer < 0 )
        {
            fireplaceHealtChangeTimer = fireplaceHealtChangeDelay;
            FireplaceHealth -= 1;
        }
    }

    public void Interact()
    {
        
    }
}
