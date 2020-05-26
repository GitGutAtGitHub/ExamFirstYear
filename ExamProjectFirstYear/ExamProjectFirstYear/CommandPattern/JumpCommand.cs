﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class JumpCommand : ICommand
	{
		public JumpCommand()
		{

		}

		public void Execute(Player player)
		{
			player.Movement.Jump();
		}
	}
}
