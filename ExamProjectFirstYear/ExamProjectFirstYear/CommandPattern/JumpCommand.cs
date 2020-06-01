using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// Jump command for the player.
	/// </summary>
	class JumpCommand : ICommand
	{
		#region Constructors

		/// <summary>
		/// Constructor for JumpCommand.
		/// </summary>
		public JumpCommand()
		{

		}

		#endregion


		#region Methods

		public void Execute(Player player)
		{
			Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			playerMovement.Jump();
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}

        #endregion
    }
}
