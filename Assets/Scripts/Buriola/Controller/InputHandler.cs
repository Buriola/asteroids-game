using UnityEngine;

namespace Buriola.Controller
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        private float _horizontal;
        private float _vertical;
        private float _fire;
        private float _delta;
        #endregion

        #region References
        private StateManager _stateManager;
        #endregion

        #region Unity Functions
        private void Start()
        {
            _stateManager = GetComponent<StateManager>();
            _stateManager.Init();
        }

        private void Update()
        {
            _delta = Time.deltaTime;
            _stateManager.Tick(_delta);
        }

        private void FixedUpdate()
        {
            _delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            _stateManager.FixedTick(_delta);
        }
        #endregion

        #region My Functions
        
        private void GetInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _fire = Input.GetAxis("Fire1");
        }
        
        private void UpdateStates()
        {
            _stateManager.Horizontal = _horizontal;
            _stateManager.Vertical = _vertical;
            _stateManager.Fire = _fire;
        }
        #endregion
    }
}
