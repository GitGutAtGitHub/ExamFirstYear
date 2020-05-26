﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class Movement : Component
    {
		#region Fields

		private float speed;
		private float momentum;

        #endregion


        #region Properties

		public static float Force { get; set; }

		public bool Grounded { get; set; }
		public bool GravityOn { get; set; }

		#endregion


		#region Contstructors

		public Movement(bool gravityOn, float momentum, float speed)
		{
			GravityOn = gravityOn;
			this.momentum = GameWorld.Instance.ScreenSize.Y / momentum;
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
				CheckGrounded();
				GravityHandling();
			}
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
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.Y && Grounded == false)
			{
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
				Force = momentum;

				GameObject.Transform.Translate(new Vector2(0, -Force));
			}
		}

		#endregion
	}
}
