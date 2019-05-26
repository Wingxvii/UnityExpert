using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public enum Runner
        {
            Player, Computer
        }

        public Runner runState;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        private void FixedUpdate()
        {

            if (runState == Runner.Computer)
            {
                AutoRunFixedUpdate();
            }
            else if(runState == Runner.Player) {
                // Read the inputs.
                bool crouch = Input.GetKey(KeyCode.LeftControl);
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                // Pass all parameters to the character control script.
                m_Character.Move(h, crouch, m_Jump);
            }


            m_Jump = false;
        }

        #region AutoRun
        private void AutoRunFixedUpdate() {

            float h = 0.0f;
            bool crouch = false;
            bool jump = false;

            //ground check
            if (m_Character.IsOnGround())
            {

            }

            m_Character.Move(h, crouch, jump);
        }


        #endregion
    }

}
