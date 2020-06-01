using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	class ReleaseCommand : ICommand
	{
		private int releaseNumber;
		public ReleaseCommand(int releaseNumber)
		{
			this.releaseNumber = releaseNumber;
		}

		public void Execute(Player player)
		{
			if (releaseNumber <= 2)
			{
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
	}
}
