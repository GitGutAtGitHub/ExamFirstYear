using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// Release command for the player.
	/// </summary>
	class ReleaseCommand : ICommand
	{
		#region Fields

		private int releaseNumber;

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for ReleaseCommand.
		/// </summary>
		/// <param name="releaseNumber"></param>
		public ReleaseCommand(int releaseNumber)
		{
			this.releaseNumber = releaseNumber;
		}

		#endregion


		#region Methods

		public void Execute(Player player)
		{
			if (releaseNumber <= 2)
			{
				((AttackMelee)player.GameObject.GetComponent(Tag.ATTACKMELEE)).ReleaseAttack(releaseNumber);
				player.ReleaseAttack(releaseNumber);
			}
            else
            {
                Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
                playerMovement.ReleaseJump();
            }

        }

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYUP;
		}

        #endregion
    }
}
