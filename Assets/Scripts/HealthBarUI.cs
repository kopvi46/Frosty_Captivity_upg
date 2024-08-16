using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasHealthGameObject;
    [SerializeField] private Image barImage;
    
    private IHasHealth hasHealth;

    private void Start()
    {
        hasHealth = hasHealthGameObject.GetComponent<IHasHealth>();
        if (hasHealth == null)
        {
            Debug.LogError("Game Object " + hasHealthGameObject + " does not have a component that implemets IHasHealth");
        }

        hasHealth.OnHealthChanged += HasHealth_OnHealthChanged;

        barImage.fillAmount = 1f;
    }

    private void HasHealth_OnHealthChanged(object sender, IHasHealth.OnHealthChangedEventArgs e)
    {
        barImage.fillAmount = e.healthNormalized;
    }
}
