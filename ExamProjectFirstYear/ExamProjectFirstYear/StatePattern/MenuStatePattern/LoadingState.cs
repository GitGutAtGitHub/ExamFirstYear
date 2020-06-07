using ExamProjectFirstYear.StatePattern;
using ExamProjectFirstYear.StatePattern.MenuStatePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
    /// <summary>
    /// The state that determines what happens when the game is loading.
    /// </summary>
    public class LoadingState : IMenuHandlerState
    {
        #region Fields

        private MenuHandler menuHandler;

        private Vector2 loadingScreenOrigin;
        private Vector2 loadingScreenPosition;
        private Vector2 loadingHeadingPosition;

        private SpriteBatch spriteBatch;

        private Texture2D loadingScreenSprite;

        private SpriteFont loadingHeading;

        private Thread backgroundThread;

        #endregion


        #region Methods

        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.LoadingState;

            menuHandler = entity as MenuHandler;

            backgroundThread = new Thread(LoadGame);

            loadingHeading = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");

            loadingScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen");

            loadingScreenOrigin = new Vector2(loadingScreenSprite.Width / 2, loadingScreenSprite.Height / 2);

            loadingScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                                GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);
            loadingHeadingPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 215,
                                                 (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 65);

            Draw();

            backgroundThread.Start();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }

        private void LoadGame()
        {
            Thread.Sleep(3000);

            menuHandler.SwitchState(new StartState());

            backgroundThread.Abort();
        }

        public void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(loadingScreenSprite, loadingScreenPosition, null, Color.White, 0f,
                             loadingScreenOrigin, 1, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(loadingHeading, "LOADING", loadingHeadingPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);

            spriteBatch.End();
        }

        #endregion
    }
}
