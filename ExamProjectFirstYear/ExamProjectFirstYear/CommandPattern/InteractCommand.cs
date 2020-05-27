using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	class InteractCommand : ICommand
	{
		public InteractCommand()
		{

		}

		public void Execute(Player player)
		{
			//TODO : kald på Players interactmetode her
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}
	}
}
