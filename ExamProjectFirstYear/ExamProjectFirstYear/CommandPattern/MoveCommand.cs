using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
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
			Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			player.Velocity = velocity;
			playerMovement.Move(velocity);
			//SoundEngine.Instance.AddSoundEffect(SoundEngine.Instance.Footsteps);

		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}

		#endregion
	}
}
