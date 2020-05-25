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
		#region Fields

		private Vector2 velocity;

		#endregion


		#region Properties

		public float Speed { get; set; }
		public float Momentum { get; set; }

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
			MovementHandling(gameTime);
		}

		private void MovementHandling(GameTime gameTime)
		{
			GameWorld.Instance.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (velocity != Vector2.Zero)
			{
				velocity.Normalize();
			}

			velocity *= Speed;

			GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
		}

		#endregion
	}
}
