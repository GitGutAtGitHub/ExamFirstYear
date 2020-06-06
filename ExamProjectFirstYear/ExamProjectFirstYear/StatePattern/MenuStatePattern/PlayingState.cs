using ExamProjectFirstYear.StatePattern;
using ExamProjectFirstYear.StatePattern.MenuStatePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class PlayingState : IMenuHandlerState
    {
        #region Fields

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private MenuHandler menuHandler;

        #endregion


        #region Methods

        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.PlayingState;

            menuHandler = entity as MenuHandler;
        }

        public void Execute()
        {
            HandleInput();
        }

        public void Exit()
        {
            
        }

        private void HandleInput()
        {
            previousKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();

            if (currentKeyBoardState.IsKeyUp(Keys.P) && previousKeyBoardState.IsKeyDown(Keys.P))
            {
                menuHandler.SwitchState(new PausedState());
            }

            if (currentKeyBoardState.IsKeyUp(Keys.E) && previousKeyBoardState.IsKeyDown(Keys.E))
            {
                GameWorld.Instance.Exit();
            }
        }

        #endregion
    }
}
