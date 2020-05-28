using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	class ReleaseCommand : ICommand
	{
		private int attackNumber;
		public ReleaseCommand(int attackNumber)
		{
			this.attackNumber = attackNumber;
		}

		public void Execute(Player player)
		{
			player.ReleaseAttack(attackNumber);
		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYUP;
		}
	}
}
