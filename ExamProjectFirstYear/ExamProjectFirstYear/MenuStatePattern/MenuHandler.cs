using ExamProjectFirstYear.MenuStatePattern;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    public enum GameState { StartState, LoadingState, PlayingState, PausedState, WonState, LostState }
    /// <summary>
    /// Class for handling menu and loadscreen logic.
    /// </summary>
    public class MenuHandler : IEntity
    {
        #region Fields

        private MenuHandler currentMenuHandler;

        private static MenuHandler instance;
        private bool startGameAtStartMenu = false;
        private bool exitGameAtMenu = false;
        private bool gameShouldBePaused = false;
        //private bool gameHasBeenExited = false;

        #endregion


        #region Properties

        public static MenuHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuHandler();
                }

                return instance;
            }
        }

        public StatePattern.IState CurrentState { get; set; }
        public GameState GameState { get; set; }
        public MenuHandler CurrentMenuHandler { get => currentMenuHandler; set => currentMenuHandler = value; }

        // used in StartState class and StartMenuCommand class. To decide what happens when certain buttons are pressed.
        public bool StartGameAtStartMenu { get => startGameAtStartMenu; set => startGameAtStartMenu = value; }

        // Used to exit the game while using menus.
        public bool ExitGameAtMenu { get => exitGameAtMenu; set => exitGameAtMenu = value; }

        // used in PausedState class and PauseMenuCommand class. When a certain button is pressed,
        // this bool becomes true and the game pauses.
        public bool GameShouldBePaused { get => gameShouldBePaused; set => gameShouldBePaused = value; }

        public bool PauseButtonReleased { get; set; }

        // Used to make sure the pause menu is only popped once at a time.

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for MenuHandler class.
        /// </summary>
        private MenuHandler()
        {

        }

        #endregion


        #region Methods

        public void SwitchState(StatePattern.IState newState)
        {
            // Makes sure the state isn't null when exiting a state.
            // This is done to avoid an exception.
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = newState;
            // "This" means the MenuHandler.
            CurrentState.Enter(this);
        }

        public void PauseOrUnPauseGame()
		{
			if (PauseButtonReleased == true)
			{
				if (GameShouldBePaused == true)
				{
                    GameShouldBePaused = false;
				}
				else
				{
                    GameShouldBePaused = true;
				}
                PauseButtonReleased = false;
			}
		}

        public void Enter(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public Tag ToTag()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
