using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _hasHealthGameObject;
    [SerializeField] private Image _barImage;
    
    private IHasHealth hasHealth;

    private void Start()
    {
        hasHealth = _hasHealthGameObject.GetComponent<IHasHealth>();
        if (hasHealth == null)
        {
            Debug.LogError("Game Object " + _hasHealthGameObject + " does not have a component that implemets IHasHealth");
        }

        hasHealth.OnHealthChanged += HasHealth_OnHealthChanged;

        _barImage.fillAmount = 1f;
    }

    private void HasHealth_OnHealthChanged(object sender, IHasHealth.OnHealthChangedEventArgs e)
    {
        _barImage.fillAmount = e.healthNormalized;
    }
}
