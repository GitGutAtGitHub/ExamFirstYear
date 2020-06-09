using ExamProjectFirstYear.MenuStatePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern.MenuCommandPattern
{
    class MenuInputHandler
    {
		#region Fields

		private Dictionary<Keys, IMenuCommand> menuKeyBinds = new Dictionary<Keys, IMenuCommand>();
		private Dictionary<Keys, IMenuCommand> menuReleaseKeyBinds = new Dictionary<Keys, IMenuCommand>();

		private static MenuInputHandler instance;

		#endregion


		#region PROPERTIES

		public static MenuInputHandler Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MenuInputHandler();
				}
				return instance;
			}
		}

		#endregion


		#region Constructors

		/// <summary>
		/// Adds all menu controls for player to a keybinds Dictionary once instantiated.
		/// </summary>
		private MenuInputHandler()
		{
			// Starts the game at the start of the game.
			menuKeyBinds.Add(Keys.S, new StartMenuCommand(1));
			// Exits the game at the start of the game.
			menuKeyBinds.Add(Keys.E, new StartMenuCommand(2));
			// Pauses the game at the press of a button.
			menuKeyBinds.Add(Keys.P, new PauseMenuCommand());

			// Makes sure the game doesn't continue to open and close the pause menu uncontrollably.
			menuReleaseKeyBinds.Add(Keys.P, new MenuReleaseCommand());
		}

		#endregion


		#region METHODS

		/// <summary>
		/// Executes all commands for the keys added to the dictionary
		/// </summary>
		public void Execute()
		{
			KeyboardState keyState = Keyboard.GetState();

			foreach (Keys key in menuKeyBinds.Keys)
			{
				if (keyState.IsKeyDown(key))
				{
					menuKeyBinds[key].Execute();
				}
			}

			foreach (Keys key in menuReleaseKeyBinds.Keys)
			{
				if (keyState.IsKeyUp(key))
				{
					menuReleaseKeyBinds[key].Execute();
				}
			}
		}

		#endregion
	}
}
