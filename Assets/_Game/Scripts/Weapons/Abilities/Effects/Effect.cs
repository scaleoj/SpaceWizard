using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using UnityAtoms;
using UnityEngine;

public class Effect : ScriptableObject, IAtomListener<GameObject>
{
   [Header("General")]
   [Tooltip("Duration in Rounds")][SerializeField] internal int duration;
   [SerializeField] internal GameObjectEvent queueGOChanged;
   [SerializeField] internal BoolVariable runUpdates;
   [SerializeField] internal bool infiniteEffect;
   [Tooltip(("NEEDS TO BE UNIQUE!"))][SerializeField] internal int effectID; //-> Unique ID of the Effect, when instantiating set the ID to the ID of the Original Object!

   [Header("Visual Representation")] 
   [SerializeField] internal Color fontColor;
   [SerializeField] internal Color dmgColor;
   [SerializeField] internal string onHitText;


   //Internal Variables
   internal bool isActive = false;
   internal Character infectedCharacter;

   //To be Overriden Methods
   public virtual void EnableEffect(GameObject target){}
   public virtual void DisableEffect(GameObject target){}
   public virtual void ApplyEffect(GameObject target){}

   //Not to be Overriden Methods
   private void OnEnable()
   {
      queueGOChanged.RegisterListener(this);
      LeftDuration = duration;
   }

   public void OnEventRaised(GameObject item)
   {
      if (!runUpdates)
      {
         return;
      }
      
      if (isActive && item.gameObject.GetComponent<Character>() == infectedCharacter)
      {
         if (!infiniteEffect)
         {
            LeftDuration--;
         }
         if (LeftDuration != 0)
         {
            ApplyEffect(item);
         }
         else
         {
            DisableEffect(item);
         }
      }
   }
   
   //Properties
   public int LeftDuration { get; set; }

   public Color FontColor
   {
      get => fontColor;
      set => fontColor = value;
   }

   public Color DmgColor
   {
      get => dmgColor;
      set => dmgColor = value;
   }

   public string OnHitText
   {
      get => onHitText;
      set => onHitText = value;
   }
}
