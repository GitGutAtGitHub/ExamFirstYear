﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
	/// <summary>
	/// The interface for handling MenuHandler states.
	/// </summary>
	public interface IMenuState
    {
		/// <summary>
		/// Is called when an Entity changes state. Contains all the setup code necessary for the state to function.
		/// </summary>
		void Enter(IEntity entity);

		/// <summary>
		/// Is run every fram in the Update method, as long as the state is active. 
		/// </summary>
		void Execute();

		/// <summary>
		/// Is called when exiting a state. Any necessary cleanup code is run here. 
		/// This method comes with the IState interface but is not used in this program.
		/// </summary>
		void Exit();
	}
}
