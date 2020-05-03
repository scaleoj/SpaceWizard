using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class SelfLight : MonoBehaviour, IAtomListener<GameObject>
{
   [SerializeField] private GameObject light;
   [SerializeField] private GameObjectEvent currentGameObjectChanged;

   private void Start()
   {
      currentGameObjectChanged.RegisterListener(this);
   }

   private void OnDestroy()
   {
      currentGameObjectChanged.UnregisterListener(this);
   }

   public GameObject Light
   {
      get => light;
      set => light = value;
   }

   public void OnEventRaised(GameObject item)
   {
      if (item == gameObject)
      {
         if (item.GetComponent<SelfLight>() != null)
         {
            item.GetComponent<SelfLight>().Light.SetActive(true);
         }  
      }
      else
      {
         Light.SetActive(false);
      }
      
   }
}
