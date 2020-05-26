using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class Movement : Component
    {
		#region Properties

		public float Speed { get; set; }
		public float Momentum { get; set; }
		public static float Force { get; set; }

		public bool Grounded { get; set; }

		#endregion


		#region Contstructors

		public Movement()
		{

		}

		#endregion


		#region Methods

		public override Tag ToEnum()
		{
			return Tag.MOVEMENT;
		}

		public override void Update(GameTime gameTime)
		{
			CheckGrounded();
			GravityHandling();
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

			velocity *= Speed;

			GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
		}

		/// <summary>
		/// Applies gravity to an object that has left the ground.
		/// </summary>
		private void GravityHandling()
		{
			/// If the players position is above the bottom of the screen and isgrounded is false (if the player is not on a platform) player will be pulled
			/// down by the value of force.
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.Y && Grounded == false)
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
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.Y)
			{
				Grounded = false;
			}

			else if (GameObject.Transform.Position.Y >= GameWorld.Instance.ScreenSize.Y)
			{
				Grounded = true;
			}
		}

		/// <summary>
		/// Enables jumping.
		/// </summary>
		public void Jump()
		{
			if (Grounded == true)
			{
				Force = Momentum;

				GameObject.Transform.Translate(new Vector2(0, -Force));
			}
		}

		#endregion
	}
}
