using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
    /// <summary>
    /// The state that determines what happens when the game is playing.
    /// </summary>
    public class PlayingState : IMenuState
    {
        #region Fields


        #endregion


        #region Methods

        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.PlayingState;

            MenuHandler.Instance.CurrentMenuHandler = entity as MenuHandler;
        }

        public void Execute()
        {
            HandlePlayingState();
            GameWorld.Instance.CheckIfWonOrLost();
        }

        public void Exit()
        {

        }

        /// <summary>
        /// Handles the playing state.
        /// If GameShouldBePaused becomes true, the game switches to PausedState and the game pauses.
        /// </summary>
        private void HandlePlayingState()
        {
            if (MenuHandler.Instance.GameShouldBePaused == true)
            {
                MenuHandler.Instance.CurrentMenuHandler.SwitchState(new PausedState());
            }
        }

        #endregion
    }
}
