using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// Interact command for player.
	/// </summary>
	class InteractCommand : ICommand
	{
		#region Constructors

		/// <summary>
		/// Constructor for InteractCommand.
		/// </summary>
		public InteractCommand()
		{

		}

		#endregion


		#region Methods

		public void Execute(Player player)
		{
			//TODO : kald på Players interactmetode her
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}

        #endregion
    }
}
