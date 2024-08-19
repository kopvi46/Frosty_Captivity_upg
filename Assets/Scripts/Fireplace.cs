using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour, IHasHealth, IInteractable
{
    public static Fireplace Instance;

    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private int fireplaceHealth = 60;
    private float fireplaceHealtChangeDelay = 3f;
    private float fireplaceHealtChangeTimer = 0f;

    public int FireplaceMaxHealth { get; private set; } = 100;
    public int FireplaceHealth
    {
        get
        {
            return fireplaceHealth;
        }
        set
        {
            int fireplaceMinHealth = 0;
            if (value < fireplaceMinHealth)
            {
                fireplaceHealth = fireplaceMinHealth;
            } else if (value > FireplaceMaxHealth)
            {
                fireplaceHealth = FireplaceMaxHealth;
            } else
            {
                fireplaceHealth = value;
            }

            OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs
            {
                healthNormalized = (float)FireplaceHealth / FireplaceMaxHealth
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

    public void Interact(Player player, Inventory inventory)
    {
        
    }
}
