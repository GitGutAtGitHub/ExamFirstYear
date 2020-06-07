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
    public enum GameState { StartState, LoadingState, PlayingState, PausedState }
    /// <summary>
    /// Class for handling menu and loadscreen logic.
    /// </summary>
    public class MenuHandler : IEntity
    {
        #region Fields

        private static MenuHandler instance;

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

        public IMenuState CurrentState { get; set; }

        public GameState GameState { get; set; }

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

        public void SwitchState(IMenuState newState)
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

        #endregion
    }
}
