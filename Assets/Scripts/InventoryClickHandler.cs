using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler OnLeftClick;
    public event EventHandler OnRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick?.Invoke(this, EventArgs.Empty);
        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
