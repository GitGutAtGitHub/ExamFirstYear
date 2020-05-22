using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class InputHandler
	{
		#region Fields
		private Dictionary<Keys, ICommand> keyBinds = new Dictionary<Keys, ICommand>();

		private static InputHandler instance;
		#endregion

		#region PROPERTIES
		public static InputHandler Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new InputHandler();
				}
				return instance;
			}
		}

		#endregion

		#region METHODS
		 /// <summary>
        /// Adds all controls for player to a keybinds Dictionary once instantiated.
        /// </summary>
        private InputHandler()
        {
            // Moves player left when pressing A.
            keyBinds.Add(Keys.A, new MoveCommand(new Vector2(-1, 0)));
            // Moves player right when pressing D.
            keyBinds.Add(Keys.D, new MoveCommand(new Vector2(1, 0)));
        }

		/// <summary>
		/// Executes all commands for the keys added to the dictionary
		/// </summary>
		/// <param name="player"></param>
		public void Execute(Player player)
		{
			KeyboardState keyState = Keyboard.GetState();

			foreach (Keys key in keyBinds.Keys)
			{
				if (keyState.IsKeyDown(key))
				{
					keyBinds[key].Execute(player);
				}
			}
		}

		#endregion
	}
}
