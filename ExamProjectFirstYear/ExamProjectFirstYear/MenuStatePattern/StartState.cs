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
    /// The state that determines what happens when the game is in the start screen.
    /// </summary>
    public class StartState : IMenuState
    {
        #region Fields

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private MenuHandler menuHandler;

        private Vector2 startScreenPosition;
        private Vector2 startTextPosition;
        private Vector2 startScreenOrigin;

        private Texture2D startScreenSprite;

        private SpriteFont startText;

        private SpriteBatch spriteBatch;

        #endregion


        #region Methods

        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.StartState;

            menuHandler = entity as MenuHandler;

            startScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen");
            startText = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");

            startTextPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 480,
                                            (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 180);

            startScreenOrigin = new Vector2(startScreenSprite.Width / 2, startScreenSprite.Height / 2);
            startScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                              GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);

            Draw();
        }

        public void Execute()
        {
            HandleInput();
        }

        public void Exit()
        {

        }

        private void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(startScreenSprite, startScreenPosition, null, Color.White, 0f,
                             startScreenOrigin, 1, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(startText, "PRESS 'S' TO START\n\n PRESS 'E' TO EXIT", startTextPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);

            spriteBatch.End();
        }

        private void HandleInput()
        {
            previousKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();

            if (currentKeyBoardState.IsKeyUp(Keys.S) && previousKeyBoardState.IsKeyDown(Keys.S))
            {
                menuHandler.SwitchState(new PlayingState());
            }

            if (currentKeyBoardState.IsKeyUp(Keys.E) && previousKeyBoardState.IsKeyDown(Keys.E))
            {
                GameWorld.Instance.GameIsRunning = false;
                GameWorld.Instance.Exit();
            }
        }

        #endregion
    }
}
