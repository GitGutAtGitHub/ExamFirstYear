using Microsoft.Xna.Framework;
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

		#endregion


		#region Properties

		public Vector2 Velocity { get; set; }
		public float Momentum { get; set; }
		public static float Force { get; set; }
		public bool Grounded { get; set; }
		public bool HasJumped { get; set; } = true;
		public bool GravityOn { get; set; }

		#endregion


		#region Contstructors

		public Movement(bool gravityOn, float maxMomentum, float speed)
		{
			GravityOn = gravityOn;
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
			if (GravityOn == true)
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
			if (Grounded == false)
			{
				/// As long as force is a higher value than -20 it will be lowered.
				/// This ensures that force will not become so low that it can pull the player throough a platform in a single frame.
				if (Force >= -20f)
				{
					Force--;
				}
				GameObject.Transform.Translate(new Vector2(0, -Force));
			}
		}

		/// <summary>
		/// Checks if the object is grounded on a platform.
		/// </summary>
		private void CheckGrounded()
		{
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.height)
			{
				Grounded = false;
			}

			else if (GameObject.Transform.Position.Y >= GameWorld.Instance.ScreenSize.height)
			{
				Grounded = true;
			}
		}

		/// <summary>
		/// Enables jumping.
		/// </summary>
		public void Jump()
		{
			if (Momentum < maxMomentum)
			{
				Momentum++;
			}

			if (Momentum >= maxMomentum)
			{
				HasJumped = true;
				Momentum = 0;
			}

			if (HasJumped == false)
			{
				Force = maxMomentum;

				GameObject.Transform.Translate(new Vector2(0, -Force));

				Grounded = false;
			}

		}

		/// <summary>
		/// VIRKER IKKE!!! Har prøvet at lave en notify for movement så den selv kan køre sin notify logik i stedet for at player skal gøre det
		/// Men skidtet virker ikke
		/// Scarcth that DET VIRKER!!!!!!!
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
						HasJumped = false;
						Momentum = 0;
					}

					//Bottom platform.
					if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
					{
						//Following ensures that players jump is interrupted if the hit a platform.
						HasJumped = true;
						Momentum = 0;
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
