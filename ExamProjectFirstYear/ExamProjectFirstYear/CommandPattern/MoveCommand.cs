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
		public MoveCommand(Vector2 velocity)
		{

		}

		/// <summary>
		/// Executes the command
		/// </summary>
		/// <param name="player"></param>
		public void Execute(Player player)
		{
			// TODO: Kald på Players Move metode her!
		}
	}
}
