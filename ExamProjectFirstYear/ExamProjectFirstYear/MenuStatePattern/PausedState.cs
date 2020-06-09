using ExamProjectFirstYear.StatePattern;
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
    /// The state that determines what happens when the game is paused.
    /// </summary>
    public class PausedState : IState
    {
        #region Fields

        private SpriteBatch spriteBatch;

        private Texture2D pausedScreenSprite;

        private Vector2 pausedHeadingPosition;
        private Vector2 pausedTextPosition;
        private Vector2 pausedScreenPosition;
        private Vector2 pausedScreenOrigin;

        private SpriteFont pausedHeading;
        private SpriteFont pausedText;

        #endregion


        #region Methods

        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.PausedState;

            MenuHandler.Instance.CurrentMenuHandler = entity as MenuHandler;

            pausedHeading = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");
            pausedText = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerText");
            pausedScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen"); ;

            pausedScreenOrigin = new Vector2(pausedScreenSprite.Width / 2, pausedScreenSprite.Height / 2);

            pausedScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                              (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2));
            pausedHeadingPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 200,
                                                (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 200);
            pausedTextPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 275,
                                             (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) + 40);

            // To ensure the game doesn't exit the moment the player enters the pause menu.
            // This is in case the player pressed the exit button by mistake while in PlayingState.
            // Without resetting this bool here, the game would exit the moment the pause button was pressed.
            MenuHandler.Instance.ExitGameAtMenu = false;

            Draw();
        }

        public void Execute()
        {
            HandlePauseState();
        }

        public void Exit()
        {

        }

        /// <summary>
        /// Handles the pause state.
        /// </summary>
        private void HandlePauseState()
        {
            //If GameShouldBePaused becomes false, the state creates a new PlayingState.
            if (MenuHandler.Instance.GameShouldBePaused == false)
            {
                MenuHandler.Instance.CurrentMenuHandler.SwitchState(new PlayingState());
            }

            // If ExitGameAtMenu becomes true while at the pause menu, the game is shut down.
            if (MenuHandler.Instance.ExitGameAtMenu == true)
            {
                GameWorld.Instance.GameIsRunning = false;
                GameWorld.Instance.Exit();
            }
        }

        /// <summary>
        /// Method used to draw the pause screen.
        /// </summary>
        public void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(pausedScreenSprite, pausedScreenPosition, null, Color.White, 0f,
                             pausedScreenOrigin, 1, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(pausedHeading, "PAUSED", pausedHeadingPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);

            spriteBatch.DrawString(pausedText, "PRESS 'P' TO UNPAUSE\n\n    PRESS 'E' TO EXIT", pausedTextPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);

            spriteBatch.End();
        }

        public Tag ToTag()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
