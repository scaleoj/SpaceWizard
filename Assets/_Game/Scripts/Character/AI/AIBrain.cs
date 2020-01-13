

using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AiBrain
    {

        private const int MELEE = 1;
        private const int RANGED = 3;
        private const int SNIPER = 5;

        private AiSenses _senses;
        private AIHub.Team _team;
        private int _range;
        private TileHub _hub;

        public AiBrain(AiSenses senses, TileHub hub, AIHub.Team team, AIHub.Range range)
        {
            _senses = senses;
            _team = team;
            switch (range)
            {
                case AIHub.Range.Ranged:
                    _range = RANGED;
                    break;
                case AIHub.Range.Sniper:
                    _range = SNIPER;
                    break;
                case AIHub.Range.Melee:
                    _range = MELEE;
                    break;
                default:
                    _range = MELEE;
                    break;   
            }

            _hub = hub;
        }
        
        //DecisionMaking
        public void MakeDecision()
        {
            if (_senses.GetRanges()[0].Value != _range)
            {
                MoveToOptimalPosition();
            }
            else
            {
                //check cooldowns
            }
            
        }

        private void MoveToOptimalPosition()
        {
           _senses.MoveToOptimalPosition();
        }
        

    }
}
