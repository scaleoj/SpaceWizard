using UnityEngine;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AiHub : MonoBehaviour
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
            Team1,
            Team2
        }
        
        private AiBrain _brain;
        private AiSenses _senses;
        private Stats.Character _character;

        [SerializeField] private TileHub hub;
        [SerializeField] private Team team;
        [SerializeField] private QueueManager queueManager;
        [SerializeField] private Team playerTeam;



        // Start is called before the first frame update
        private void Start()
        {
            _state = AiState.Asleep;
            _character = GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            if (team == playerTeam) return;
            while (hub == null)
            {
                _senses = new AiSenses(hub, queueManager, _character);
                Debug.Log(gameObject.name + ": No TileHub found!");
            }
            _brain = new AiBrain(_senses, team);

        }

        public void PositionUpdate()
        {
            _senses.UpdateRanges();
        }

    }
}
