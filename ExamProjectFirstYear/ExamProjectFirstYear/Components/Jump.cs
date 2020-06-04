using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class Jump : Component, IGameListener
    {
		Player player;
		Movement playerMovement;
		private float maxMomentum;
        private float momentum;
		private bool hasJumped = true;


        public Jump(float maxMomentum)
		{
			player = GameWorld.Instance.player;
			playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			this.maxMomentum = maxMomentum;
        }


        /// <summary>
        /// Enables jumping.
        /// </summary>
        public void PlayerJump(Movement playerMovement)
        {
            if (momentum < maxMomentum)
            {
                momentum += 5;
            }

            if (momentum >= maxMomentum)
            {
                hasJumped = true;
                momentum = 0;
            }

            if (hasJumped == false)
            {
				playerMovement.Force = momentum;
				GameObject.Transform.Translate(new Vector2(0, -momentum));
            }
        }

        /// <summary>
        /// Sets Jump to true.
        /// </summary>
        public void ReleaseJump()
        {
            hasJumped = false;
        }

        public override Tag ToEnum()
        {
            return Tag.JUMP;
        }

		public void Notify(GameEvent gameEvent, Component component)
		{
			if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLATFORM)
			{
				Rectangle intersection = Rectangle.Intersect(((Collider)(component.GameObject.GetComponent(Tag.COLLIDER))).CollisionBox,
									((Collider)(GameObject.GetComponent(Tag.COLLIDER))).CollisionBox);

				//Top and bottom platform.
				if (intersection.Width > intersection.Height)
				{
					//Top platform.
					if (component.GameObject.Transform.Position.Y > GameObject.Transform.Position.Y)
					{
						//Following ensures that player can jump again when landing on top of a platform.

						playerMovement.Grounded = true;
						momentum = 0;
						hasJumped = false;
					}

					//Bottom platform.
					if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
					{
						//Following ensures that players jump is interrupted if the hit a platform.
						momentum = 0;
						hasJumped = true;
					}
				}
			}

			if (gameEvent.Title == "NoLongerColliding" && component.GameObject.Tag == Tag.PLATFORM)
			{
				playerMovement.Grounded = false;
			}
		}
	}
}
