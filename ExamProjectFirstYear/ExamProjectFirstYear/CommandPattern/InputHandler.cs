using ExamProjectFirstYear.CommandPattern;
using ExamProjectFirstYear.CommandPattern.MenuCommandPattern;
using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	/// <summary>
	/// InputHander class.
	/// </summary>
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


		#region Constructors

		/// <summary>
		/// Adds all controls for player to a keybinds Dictionary once instantiated.
		/// </summary>
		private InputHandler()
		{
			// HVIS GODMODE ER SLÅET TIL
			//keyBinds.Add(Keys.Up, new MoveCommand(new Vector2(0, -1)));
			//keyBinds.Add(Keys.Down, new MoveCommand(new Vector2(0, 1)));

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

			// Opens/closes the journal.
			keyBinds.Add(Keys.J, new JournalCommand(1));
			// Changes the page of the journal.
			keyBinds.Add(Keys.K, new JournalCommand(2));
			// Opens inventory
			keyBinds.Add(Keys.I, new InventoryCommand());

			// Releasecommands are tasked with running any code that relies on a button not being pressed
			releaseKeyBinds.Add(Keys.X, new ReleaseCommand(1));
			releaseKeyBinds.Add(Keys.Z, new ReleaseCommand(2));
			releaseKeyBinds.Add(Keys.Space, new ReleaseCommand(3));
			releaseKeyBinds.Add(Keys.Right, new ReleaseCommand(4));
			releaseKeyBinds.Add(Keys.Left, new ReleaseCommand(5)); 
			releaseKeyBinds.Add(Keys.J, new ReleaseCommand(6));
			releaseKeyBinds.Add(Keys.K, new ReleaseCommand(7));
			releaseKeyBinds.Add(Keys.I, new ReleaseCommand(8));
		}

        #endregion


        #region METHODS

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
