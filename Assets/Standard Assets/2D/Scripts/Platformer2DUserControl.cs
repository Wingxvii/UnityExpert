using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/*
    User controll based on instructor written version with added AI running mechanics
   
    - Original by Dustin Carroll
    - Version 2 by John Wang
*/

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
                m_Jump = false;
            }


        }


        #region AutoRun

        public LayerMask hitMask = (1 << 8) | (1 << 0);     //default mask and enemy mask
        public float forwardCheckOffset = 0.3f;
        public float checkDown = 10.0f;
        public float checkNextStep = 1.0f;

        private void AutoRunFixedUpdate() {

            float h = 0.0f;
            bool crouch = false;
            bool jump = false;

            //used to check if theres an obstacle forward from the head
            RaycastHit2D headHit = Physics2D.Raycast(m_Character.m_CeilingCheck.position + forwardCheckOffset * Vector3.right, Vector2.right, checkNextStep, hitMask);

            //used to check if theres an obstacle forward from the leg
            RaycastHit2D legHit = Physics2D.Raycast(m_Character.m_GroundCheck.position + forwardCheckOffset * Vector3.right + 0.25f * Vector3.up, Vector2.right, checkNextStep, hitMask);

            //used to check if theres more ground to walk on
            RaycastHit2D walkCheck = Physics2D.Raycast(m_Character.m_GroundCheck.position + forwardCheckOffset * Vector3.right, Vector2.right, checkNextStep, hitMask);

            //used to check if there is something to land on
            RaycastHit2D dropCheck = Physics2D.Raycast(m_Character.m_GroundCheck.position, Vector2.down, checkDown, hitMask);

            //used to check if there is something to land on further ahead
            RaycastHit2D forwardDropCheck = Physics2D.Raycast(m_Character.m_GroundCheck.position + forwardCheckOffset * Vector3.right, Vector2.down, checkDown, hitMask);


            //Very simple AI system that prioritises speed

            //assume AI always wants to advance
            h = 1.0f;
            //this finds out when its appropriate to jump
            if (m_Character.IsOnGround() && (walkCheck.collider == null || headHit.collider != null || legHit.collider != null)) {
                jump = true;
            }
            //this finds out where the robot cannot land, and stops them if they are about to go there
            if (!m_Character.IsOnGround() && dropCheck.collider != null && forwardDropCheck.collider == null) {
                h = 0.0f;  
            }                
            //this tells the robot that its head is stopping it from moving forward
            if (headHit.collider != null && legHit.collider == null) {
                crouch = true;
            }

            //movement call
            m_Character.Move(h, crouch, jump);
        }


        #endregion
    }

}
