

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
                    _range = Ranged;
                    break;
                case AIHub.Range.Sniper:
                    _range = Sniper;
                    break;
                case AIHub.Range.Melee:
                    _range = Melee;
                    break;
                default:
                    _range = Melee;
                    break;   
            }

            _hub = hub;
        }
        
        //DecisionMaking
        public void MakeDecision()
        {
            var actionsleft = true;
            while (actionsleft)
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
            

            
            //endTurn einbauen
            
        }

        private void MoveToOptimalPosition()
        {
           _senses.MoveToOptimalPosition();
        }
        

    }
}
