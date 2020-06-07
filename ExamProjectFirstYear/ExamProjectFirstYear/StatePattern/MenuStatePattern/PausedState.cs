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
    /// The state that determines what happens when the game is paused.
    /// </summary>
    public class PausedState : IMenuHandlerState
    {
        #region Fields

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private MenuHandler menuHandler;

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

            menuHandler = entity as MenuHandler;

            pausedHeading = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");
            pausedText = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerText");
            pausedScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen");;

            pausedScreenOrigin = new Vector2(pausedScreenSprite.Width / 2, pausedScreenSprite.Height / 2);

            pausedScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                              (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2));
            pausedHeadingPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 200,
                                                (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 200);
            pausedTextPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 275,
                                             (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) + 40);

            Draw();
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
                menuHandler.SwitchState(new PlayingState());
            }

            if (currentKeyBoardState.IsKeyUp(Keys.E) && previousKeyBoardState.IsKeyDown(Keys.E))
            {
                GameWorld.Instance.GameIsRunning = false;
                GameWorld.Instance.Exit();
            }
        }

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

        #endregion
    }
}
