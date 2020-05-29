﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	public class Movement : Component, IGameListener
	{
		#region Fields

		private float speed;
		private float maxMomentum;
		private bool hasJumped = true;
		private bool gravityOn;
		private static float force;
		private float momentum;
		#endregion


		#region Properties

		public Vector2 Velocity { get; set; }
		public bool Grounded { get; set; }

		#endregion


		#region Contstructors

		public Movement(bool gravityOn, float maxMomentum, float speed)
		{
			this.gravityOn = gravityOn;
			this.maxMomentum = maxMomentum;
			this.speed = speed;

		}

		#endregion


		#region Methods

		public override Tag ToEnum()
		{
			return Tag.MOVEMENT;
		}

		public override void Update(GameTime gameTime)
		{
			if (gravityOn == true)
			{
				GravityHandling();
			}
			Move(Velocity);
		}

		/// <summary>
		/// Enables movement of the object.
		/// </summary>
		/// <param name="velocity"></param>
		public void Move(Vector2 velocity)
		{
			if (velocity != Vector2.Zero)
			{
				velocity.Normalize();
			}

			velocity *= speed;

			GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
		}

		/// <summary>
		/// Applies gravity to an object that has left the ground.
		/// </summary>
		private void GravityHandling()
		{
			/// If the players position is above the bottom of the screen and isgrounded is false (if the player is not on a platform) player will be pulled
			/// down by the value of force.
			if (Grounded == false && hasJumped == true)
			{
				/// As long as force is a higher value than -20 it will be lowered.
				/// This ensures that force will not become so low that it can pull the player throough a platform in a single frame.
				if (force >= -20f)
				{
					force--;
				}
				GameObject.Transform.Translate(new Vector2(0, -force));
			}
			if (Grounded == true)
			{
				force = 0;
			}
		}

		/// <summary>
		/// Enables jumping.
		/// </summary>
		public void Jump()
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
				force = momentum;

				GameObject.Transform.Translate(new Vector2(0, -momentum));

				Grounded = false;
			}

			//This is if the jump should never change and always be a constant height
			//if (Grounded == true && HasJumped == false)
			//{
			//	Force = maxMomentum;

			//	GameObject.Transform.Translate(new Vector2(0, -Force));

			//	Grounded = false;
			//}

		}

		public void ReleaseJump()
		{
			hasJumped = true;
		}

		/// <summary>
		/// Relevant for player. Notifies when player collides with a platform. Ensures that Grounded is set to true and hasJumped to false
		/// When landing on a platform, so that player may jump again.
		/// Ensures that gravity kicks in and pulls player down if player jumps up and collides with a platform from below. 
		/// </summary>
		/// <param name="gameEvent"></param>
		/// <param name="component"></param>
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
						Grounded = true;
						hasJumped = false;
						momentum = 0;
					}

					//Bottom platform.
					if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
					{
						//Following ensures that players jump is interrupted if the hit a platform.
						hasJumped = true;
						momentum = 0;
					}
				}
			}
			if (gameEvent.Title == "NoLongerColliding" && component.GameObject.Tag == Tag.PLATFORM)
			{
				Grounded = false;
			}

		}


		#endregion
	}
}
