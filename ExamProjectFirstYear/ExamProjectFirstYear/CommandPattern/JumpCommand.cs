using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	class JumpCommand : ICommand
	{
		public JumpCommand()
		{

		}

		public void Execute(Player player)
		{
			//player.Movement.Jump();
			Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			//playerMovement.ManageMomentum();
			playerMovement.Jump();
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}
	}
}
