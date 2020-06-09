using ExamProjectFirstYear.StatePattern;
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
	public class LostState : IState
    {
        private MenuHandler menuHandler;

        private Texture2D lostScreenSprite;

        private Vector2 lostScreenPosition;
        //private Vector2 lostTextPosition;
        private Vector2 lostScreenOrigin;
        private Vector2 lostHeadingPosition;

        //private SpriteFont lostText;
        private SpriteFont lostHeading;

        private SpriteBatch spriteBatch;

        private Thread backgroundThread;


        public void Enter(IEntity entity)
        {
            MenuHandler.Instance.GameState = GameState.LostState;

            menuHandler = entity as MenuHandler;

            backgroundThread = new Thread(LoadGame);

            lostScreenSprite = GameWorld.Instance.Content.Load<Texture2D>("OopGameScreen");
            //lostText = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerText");
            lostHeading = GameWorld.Instance.Content.Load<SpriteFont>("MenuHandlerHeading");

            lostScreenOrigin = new Vector2(lostScreenSprite.Width / 2, lostScreenSprite.Height / 2);

            lostScreenPosition = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2,
                                              GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);
            lostHeadingPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 250,
                                                (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) - 65);
            //lostTextPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2) - 275,
            //                                 (GameWorld.Instance.GraphicsDevice.Viewport.Height / 2) + 40);

            Draw();

            backgroundThread.Start();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }

        public Tag ToTag()
        {
            throw new NotImplementedException();
        }

        private void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(lostScreenSprite, lostScreenPosition, null, Color.White, 0f,
                             lostScreenOrigin, 1, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(lostHeading, "YOU LOST", lostHeadingPosition,
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);
            //spriteBatch.DrawString(lostText, "PRESS 'S' TO START\n\n PRESS 'E' TO EXIT", lostTextPosition,
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
