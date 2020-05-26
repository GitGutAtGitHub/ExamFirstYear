using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class AttackCommand : ICommand
	{
		private int attackNumber;
		public AttackCommand(int attackNumber)
		{
			this.attackNumber = attackNumber;
		}

		public void Execute(Player player)
		{
			player.Attack(attackNumber);
		}
	}
}
