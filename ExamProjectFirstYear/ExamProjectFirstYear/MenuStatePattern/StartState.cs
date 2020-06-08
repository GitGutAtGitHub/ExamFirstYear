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

            MenuHandler.Instance.CurrentMenuHandler = entity as MenuHandler;

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
            StartGameMenuHandler();
        }

        public void Exit()
        {

        }

        /// <summary>
        /// Method used to draw the start screen.
        /// </summary>
        private void Draw()
        {
            spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            spriteBatch.Begin();

            spriteBatch.Draw(startScreenSprite, startScreenPosition, null, Color.White, 0f,
                             startScreenOrigin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.98f);

            spriteBatch.DrawString(startText, "PRESS 'S' TO START\n\n PRESS 'E' TO EXIT", startTextPosition,
                                   Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.99f);

            spriteBatch.End();
        }

        /// <summary>
        /// Depending on the button the player presses, the game switches to a new state or the game shuts down.
        /// This has been set up with bools, which becomes true in the StartMenuCommand class,
        /// when the correct buttons are pressed.
        /// </summary>
        private void StartGameMenuHandler()
        {
            if (MenuHandler.Instance.StartGameAtStartMenu == true)
            {
                // Starts the game. Enters PlayingState.
                MenuHandler.Instance.CurrentMenuHandler.SwitchState(new PlayingState());
            }

            if (MenuHandler.Instance.ExitGameAtMenu == true)
            {
                // Exits/closes the game.
                GameWorld.Instance.GameIsRunning = false;
                GameWorld.Instance.Exit();
            }
        }

        #endregion
    }
}
