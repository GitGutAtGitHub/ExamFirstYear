using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
	public class WonState : IMenuState
	{
        private MenuHandler menuHandler;

        private Texture2D wonScreenSprite;

        private Vector2 wonScreenPosition;
        //private Vector2 wonTextPosition;
        private Vector2 wonScreenOrigin;
        private Vector2 wonHeadingPosition;

        //private SpriteFont wonText;
        private SpriteFont wonHeading;

        private SpriteBatch spriteBatch;

        private Thread backgroundThread;


        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.WonState;

            menuHandler = entity as MenuHandler;

            backgroundThread = new Thread(LoadGame);

            wonScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen");
            //wonText = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerText");
            wonHeading = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");

            wonScreenOrigin = new Vector2(wonScreenSprite.Width / 2, wonScreenSprite.Height / 2);

            wonScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                              GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);
            wonHeadingPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 240,
                                                (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 65);
            //wonTextPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 275,
            //(GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) + 40);

            Draw();

            backgroundThread.Start();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }

        private void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(wonScreenSprite, wonScreenPosition, null, Color.White, 0f,
                             wonScreenOrigin, 1, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(wonHeading, "YOU WON", wonHeadingPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);
            //spriteBatch.DrawString(wonText, "PRESS 'S' TO START\n\n PRESS 'E' TO EXIT", wonTextPosition,
            //                       Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);

            spriteBatch.End();
        }

        private void LoadGame()
        {
            Thread.Sleep(5000);

            menuHandler.SwitchState(new LoadingState());

            backgroundThread.Abort();
        }
    }
}
