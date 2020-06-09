using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// MoveCommand for the player.
	/// </summary>
	class MoveCommand : ICommand
	{
		#region Fields

		private Vector2 velocity;

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for MoveCommand.
		/// </summary>
		/// <param name="velocity"></param>
		public MoveCommand(Vector2 velocity)
		{
			this.velocity = velocity;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="player"></param>
		public void Execute(Player player)
		{
			player.PlayerVelocity = velocity;
			player.Movement.Move(velocity);

			player.SpriteRenderer.FlipSprite(velocity);
			if (player.Movement.Grounded == true)
			{
				player.SpriteRenderer.AnimationOn = true;
			}
			if (velocity.X > 0)
			{
				player.AnimationHandler.MovingRight = true;
			}
			if (velocity.X < 0)
			{
				player.AnimationHandler.MovingLeft = true;
			}

			//player.SoundComponent.ChangeRepeat("footstepsLouder", true);
			//SoundEngine.Instance.AddSoundEffect(SoundEngine.Instance.Footsteps);
		}
		#endregion
	}
}
