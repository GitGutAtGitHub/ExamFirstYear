using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
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
			player.Movement.Move(velocity);
			player.Direction = velocity;
		}

        #endregion
    }
}
