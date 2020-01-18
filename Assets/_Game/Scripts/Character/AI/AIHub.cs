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


        
        private AiBrain _brain;
        private AiSenses _senses;
        private Stats.Character _character;

        [SerializeField] private TileHub hub;
        [SerializeField] private QueueManager queueManager;


        // Start is called before the first frame update
        private void Start()
        {
            _state = AiState.Asleep;
            
            _character = GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            if (_character.CharStats.Team == 0) return;
            while (hub == null)
            {   
                Debug.Log(gameObject.name + ": No TileHub found!");
            }
            _senses = new AiSenses(hub, queueManager, _character);
            _brain = new AiBrain(_senses, hub, _character.CharStats.Team, _character.CharStats.MChartype);

        }

        public void PositionUpdate()
        {
            _senses.UpdateRanges();
        }

    }
}
