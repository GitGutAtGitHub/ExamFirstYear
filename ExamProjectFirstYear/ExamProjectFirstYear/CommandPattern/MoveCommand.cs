using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	class MoveCommand : ICommand
	{
		#region Fields

		private Vector2 velocity;

		#endregion


		#region Constructors

		public MoveCommand(Vector2 velocity)
		{
			this.velocity = velocity;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Executes the command
		/// </summary>
		/// <param name="player"></param>
		public void Execute(Player player)
		{
			//player.Movement.Move(velocity);

			Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			player.Direction = velocity;
			playerMovement.Move(velocity);
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}
		#endregion
	}
}
