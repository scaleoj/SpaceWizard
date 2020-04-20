using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using UnityAtoms;
using UnityEngine;

public class CharacterKiller : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent killChar;
    [SerializeField] private QueueManager queue;
    [SerializeField] private VoidEvent updateHUD;

    private void Awake()
    {
        killChar.RegisterListener(this);
    }

    public void OnEventRaised(GameObject item)
    {
        StartCoroutine(killWaitTime(item));
    }

    private IEnumerator killWaitTime(GameObject item)
    {
        yield return new WaitForSeconds(1);
        //updateHUD.Raise();
        queue.KillUnit(item);
        item.transform.position = Vector3.one * 1000;
        Character itemChar = item.GetComponent<Character>();
        itemChar.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
        itemChar.OccupiedTile = null;
        //Disable all effects
        if (itemChar.CharStats.ActiveEffects.Count != 0)
        {
            for (int i = itemChar.CharStats.ActiveEffects.Count - 1; 0 <= i; i--)
            {
                itemChar.CharStats.ActiveEffects[i].DisableEffect(item);
            }
        }
    }
}
