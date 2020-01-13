using UnityEngine;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AIHub : MonoBehaviour
    {
        private AiState _state;
        
        public enum AiState
        {
            Awake,
            Asleep
        }

        public AiState _State
        {
            get => _state;
            set => _state = value;
        }

        public enum Team
        {
            Team1, //Playerteam
            Team2
        }

        public enum Range
        {
            Melee,
            Ranged,
            Sniper
        }


        
        private AiBrain _brain;
        private AiSenses _senses;
        private Stats.Character _character;
        private Range _range;

        [SerializeField] private TileHub hub;
        [SerializeField] private Team team;
        [SerializeField] private QueueManager queueManager;
        [SerializeField] private Range range;



        // Start is called before the first frame update
        private void Start()
        {
            _state = AiState.Asleep;
            
            _character = GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            if (team == Team.Team1) return;
            while (hub == null)
            {   
                Debug.Log(gameObject.name + ": No TileHub found!");
            }
            _senses = new AiSenses(hub, queueManager, _character);
            _brain = new AiBrain(_senses, hub, team, range);

        }

        public void PositionUpdate()
        {
            _senses.UpdateRanges();
        }

    }
}
