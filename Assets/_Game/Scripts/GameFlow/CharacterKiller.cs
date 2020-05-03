﻿using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using UnityAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterKiller : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent killChar;
    [SerializeField] private QueueManager queue;
    [SerializeField] private VoidEvent updateHUD;
    [SerializeField] private StringEvent alertBox;

    private void Awake()
    {
        killChar.RegisterListener(this);
    }

    private void OnDestroy()
    {
        killChar.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject item)
    {
        StartCoroutine(killWaitTime(item));
    }

    private IEnumerator killWaitTime(GameObject item)
    {
        yield return new WaitForSeconds(1);
        //updateHUD.Raise();
        Debug.Log(item);
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

        bool endGame = false;
        int firstFoundParty = queue.Queue[0].Key.GetComponent<Character>().CharStats.Team;

        for (int i = 1; i < queue.Queue.Count; i++)
        {
            if (queue.Queue[i].Key.GetComponent<Character>().CharStats.Team != firstFoundParty)
            {
                endGame = false;
                Debug.Log("Dont end the Game!");
                break;
            }
            else
            {
                Debug.Log("End The Game!");
                endGame = true;
            }
        }

        if (endGame)
        {
            yield return new WaitForSeconds(0.2f);
            if (firstFoundParty == 0)
            {
                alertBox.Raise("Congratulations: Game won");
            }
            else
            {
                alertBox.Raise("Game Over");
            }
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("MainMenu");
        }
            
    }
}
