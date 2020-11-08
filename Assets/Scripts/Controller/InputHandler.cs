using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Controller
{
    /// <summary>
    /// This class will handle all input from the player and update it to the StateManager
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        float horizontal;
        float vertical;
        float fire;
        float delta;
        #endregion

        #region References
        StateManager states;
        #endregion

        #region Unity Functions
        private void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();
        }

        private void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
        }

        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
        }
        #endregion

        #region My Functions

        /// <summary>
        /// Gets the inputs from the axis and stores them in the local variables
        /// </summary>
        private void GetInput()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            fire = Input.GetAxis("Fire1");
        }

        /// <summary>
        /// Pass the input values to the state manager
        /// </summary>
        private void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;
            states.fire = fire;
        }
        #endregion
    }
}
