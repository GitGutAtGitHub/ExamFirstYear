using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class Jump : Component
    {
        Player player;
        Movement playerMovement;


        public Jump()
        {
            player = GameWorld.Instance.player;
            playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
        }


        /// <summary>
        /// Enables jumping.
        /// </summary>
        public void PlayerJump()
        {
            if (playerMovement.Momentum < playerMovement.MaxMomentum)
            {
                playerMovement.Momentum += 5;
            }

            if (playerMovement.Momentum >= playerMovement.MaxMomentum)
            {
                playerMovement.HasJumped = true;
                playerMovement.Momentum = 0;
            }

            if (playerMovement.HasJumped == false)
            {
                playerMovement.Force = playerMovement.Momentum;

                player.GameObject.Transform.Translate(new Vector2(0, -playerMovement.Momentum));

                playerMovement.Grounded = false;
            }


            //This is if the jump should never change and always be a constant height
            //if (Grounded == true && HasJumped == false)
            //{
            //	Force = maxMomentum;

            //	GameObject.Transform.Translate(new Vector2(0, -Force));

            //	Grounded = false;
            //}
        }

        /// <summary>
        /// Sets Jump to true.
        /// </summary>
        public void ReleaseJump()
        {
            playerMovement.HasJumped = true;
        }

        public override Tag ToEnum()
        {
            return Tag.JUMP;
        }
    }
}
