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
			if (player.PlayerCollidingWithDoor == true)
			{
				if (player.AllMaterialsCollected == true)
				{
					LevelManager.Instance.Door.OpenDoor();
				}

				// If there is time for a pop-up, this can be added!
				//else
				//{
				//    // Pop-up - door is locked?
				//}
			}
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}

        #endregion
    }
}
