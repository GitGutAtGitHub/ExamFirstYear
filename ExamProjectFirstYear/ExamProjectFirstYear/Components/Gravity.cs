using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	/// <summary>
	/// This class handles gravity for game objects.
	/// </summary>
    public class Gravity : Component
    {
		#region Fields

		private Movement movement;
		private SpriteRenderer spriteRenderer;

		#endregion


		#region Properties

		public static float Force { get; set; }
		public bool Grounded { get; set; }

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for GravityComponent class.
		/// </summary>
		public Gravity()
		{

		}

        #endregion


        #region Methods

        public override Tag ToEnum()
		{
			return Tag.GRAVITY;
		}

		public override void Start()
		{
			movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
			spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
		}

		public override void Update(GameTime gameTime)
		{
			CheckGrounded();
			GravityHandling();
		}

		/// <summary>
		/// Applies gravity to an object that has left the ground.
		/// </summary>
		private void GravityHandling()
		{
			/// If the players position is above the bottom of the screen and isgrounded is false (if the player is not on a platform) player will be pulled
			/// down by the value of force.
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.Y - spriteRenderer.Sprite.Height / 2 && Grounded == false)
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

		private void CheckGrounded()
		{
			if (GameObject.Transform.Position.Y < GameWorld.Instance.ScreenSize.Y - spriteRenderer.Sprite.Height / 2)
			{
				Grounded = false;
			}

			else if (GameObject.Transform.Position.Y >= GameWorld.Instance.ScreenSize.Y - spriteRenderer.Sprite.Height / 2)
			{
				Grounded = true;
			}
		}

		#endregion
	}
}
