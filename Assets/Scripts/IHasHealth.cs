using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasHealth
{
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public float healthNormalized;
    }
}
