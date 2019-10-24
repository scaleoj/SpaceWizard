using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class Stats : MonoBehaviour
{
   [SerializeField] private IntVariable currentValue;
   [SerializeField] private IntVariable maxValue;

   public IntVariable CurrentValue
   {
      get => currentValue;
      set => CurrentValue.Value = Mathf.Clamp(value.Value, 0, maxValue.Value);
   }
}
