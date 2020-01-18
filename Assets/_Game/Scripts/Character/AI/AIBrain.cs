

using UnityEditor.Tilemaps;
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

        public AiBrain(AiSenses senses, TileHub hub, Stats.Character character)
        {
            _senses = senses;
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
        public void MakeDecision()
        {
            var actionsleft = true;
            while (actionsleft)
            {
                if (_senses.GetRanges()[0].Value < _range)
                {
                    Retreat();
                    
                }
                else if(_senses.GetRanges()[0].Value >= _range)
                {
                    Move();
                }
            }
            

            
            //endTurn einbauen
            
        }

        private void Retreat()
        {
           _senses.Retreat();
        }

        private void Move()
        {
            _senses.Move();
        }
        

    }
}
