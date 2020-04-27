﻿using System.Collections;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AiBrain
    {
        private const int Melee = 1;
        private const int Ranged = 3;
        private const int Sniper = 5;

        private AiSenses _senses;
        private int _team;
        private int _range;
        private Stats.Character _character;
        private bool hasAttacked;

        public AiBrain(AiSenses senses, TileHub hub, Stats.Character character)
        {
            _senses = senses;
            _character = character;
            _team = character.CharStats.Team;
            switch (character.CharStats.MChartype)
            {
                case CharacterStat.CharType.Base:
                    _range = Ranged;
                    break;
                case CharacterStat.CharType.Sniper:
                    _range = Sniper;
                    break;
                case CharacterStat.CharType.Melee:
                    _range = Melee;
                    break;
                case CharacterStat.CharType.Support:
                    _range = Ranged;
                    break;
                case CharacterStat.CharType.Tank:
                    _range = Melee;
                    break;
                default:
                    _range = Melee;
                    break;
            }
        }

        //DecisionMaking
        public IEnumerator MakeDecision()
        {
            yield return new WaitForSeconds(2f);
            /*yield return new WaitForSeconds(time)
             yield return null <- wait one frame*/
            hasAttacked = false;
            var range = 0;
            Stats.Character target = null;
            while (_senses.ApCount() > 0)
            {
                yield return new WaitForSeconds(0.5f);
                while (AiSenses.isMoving)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                _senses.UpdateRanges();
                foreach (var ch in _senses.GetRanges())
                {
                    if (ch.Key.GetComponent<Stats.Character>().CharStats.Team == _team) continue;
                    target = ch.Key.GetComponent<Stats.Character>();
                    range = ch.Value;
                    break;
                }

                if (target == null)
                {
                    break;
                }


                if (range < _range)
                {
                    Debug.Log("retreat");
                    Retreat();
                    continue;
                }

                if (range > _range)
                {
                    Debug.Log("move");
                    Debug.Log("Rangelimit: " + _range + "/ Range: " + range);
                    Move(target);
                    continue;
                }

                if (_character.CharStats.PrimaryWeapon.Abilities[0].ApCost < _senses.ApCount() && !hasAttacked)
                {
                    Debug.Log("attack");
                    _character.CharStats.PrimaryWeapon.Abilities[0].Attack(target.gameObject, range);
                    hasAttacked = true;
                }
                else
                {
                    Debug.Log("Break");
                    Debug.Log("Rangelimit: " + _range + "/ Range: " + range);
                    break;
                }

                
                /* 
                
                for (var i = 0; i < _character.CharStats.PrimaryWeapon.Abilities.Length-1; i=i)
                {
                if (_senses.ApCount() >= _character.CharStats.PrimaryWeapon.Abilities[i].ApCost)
                {
                if (_character.CharStats.PrimaryWeapon.Range <= _senses.GetRanges()[0].Value)
                {
                _character.CharStats.PrimaryWeapon.Abilities[i].Attack(_senses.GetRanges()[0].Key, _senses.GetRanges()[0].Value);
                }
                else
                {
                i++;
                }
                }
                else
                {
                i++;
                }
                }*/

                //_senses.DecreaseAP();
            }
            _senses.Next();
        }

        private void Retreat()
        {
            _senses.Retreat();
        }

        private void Move(Stats.Character target)
        {
            _senses.Move(target);
        }
    }
}