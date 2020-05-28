using ExamProjectFirstYear.CommandPattern;
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
		private Dictionary<Keys, ICommand> releaseKeyBinds = new Dictionary<Keys,ICommand>();

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
            keyBinds.Add(Keys.Left, new MoveCommand(new Vector2(-1, 0)));
            // Moves player right when pressing D.
            keyBinds.Add(Keys.Right, new MoveCommand(new Vector2(1, 0)));
			// Player jumps when pressing space.
			keyBinds.Add(Keys.Space, new JumpCommand());
			// Player uses a melee attack when pressing x.
			keyBinds.Add(Keys.X, new AttackCommand(1));
			// Player uses a ranged attack when pressing z.
			keyBinds.Add(Keys.Z, new AttackCommand(2));
			// Player interacts when pressing a.
			keyBinds.Add(Keys.A, new InteractCommand());

			// Player releases the meleeattack
			releaseKeyBinds.Add(Keys.X, new ReleaseCommand(1));
			// Player releases the rangedattack
			releaseKeyBinds.Add(Keys.Z, new ReleaseCommand(2));
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
				//if (keyBinds[key].GetCommandTag() == CommandTag.KEYUP)
				//{
				//	if (keyState.IsKeyUp(key))
				//	{
				//		keyBinds[key].Execute(player);
				//	}
				//}
				//else if (keyBinds[key].GetCommandTag() == CommandTag.KEYDOWN)
				//{
				//	if (keyState.IsKeyDown(key))
				//	{
				//		keyBinds[key].Execute(player);
				//	}
				//}
				if (keyState.IsKeyDown(key))
				{
					keyBinds[key].Execute(player);
				}
			}
			foreach (Keys key in releaseKeyBinds.Keys)
			{
				if (keyState.IsKeyUp(key))
				{
					releaseKeyBinds[key].Execute(player);
				}
			}
		}

		#endregion
	}
}
