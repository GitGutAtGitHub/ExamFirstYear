using ExamProjectFirstYear.CommandPattern;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// Interface for command classes.
	/// </summary>
	interface ICommand
	{
		void Execute(Player player);
	}
}
