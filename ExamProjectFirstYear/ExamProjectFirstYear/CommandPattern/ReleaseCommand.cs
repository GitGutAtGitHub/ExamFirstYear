using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using Microsoft.Xna.Framework;
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
			// releaseNumber is used to make sure this method is only called, when a certain attack button is released.
			if (releaseNumber == 1)
			{
				AttackMelee playerMelee = (AttackMelee)player.GameObject.GetComponent(Tag.ATTACKMELEE);
				playerMelee.ReleaseMeleeMeleeAttack();
			}
			else if (releaseNumber == 2)
			{
				player.PlayerReleaseRangedAttack();
			}
			else if (releaseNumber == 3)
			{
				Jump playerJump = (Jump)player.GameObject.GetComponent(Tag.JUMP);
				playerJump.ReleaseJump();
			}
			else if (releaseNumber == 6)
			{
				GameWorld.Instance.Journal.CanOperateJournal = true;
			}
			else if (releaseNumber == 7)
			{
				GameWorld.Instance.Journal.CanChangePage = true;
			}
			else if (releaseNumber == 8)
			{
				GameWorld.Instance.Inventory.CanOperateInventory = true;
			}

		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYUP;
		}

        #endregion
    }
}
