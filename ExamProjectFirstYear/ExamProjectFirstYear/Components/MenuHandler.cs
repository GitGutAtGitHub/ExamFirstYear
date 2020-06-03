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
    /// <summary>
    /// Class for handling menu and loadscreen logic.
    /// </summary>
    public class MenuHandler : Component, IEntity
    {
        #region Fields

        private Texture2D startSprite;
        private Texture2D exitSprite;
        private Texture2D pauseSprite;
        private Texture2D resumeSprite;
        private Texture2D loadingScreenSprite;

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private Thread backgroundThread;

        private bool Loading = false;

        private IState currentState;

        #endregion


        #region Properties

        public Texture2D StartSprite { get; set; }
        public Texture2D ExitSprite { get; set; }
        public Texture2D PauseSprite { get; set; }
        public Texture2D ResumeSprite { get; set; }
        public Texture2D LoadingScreenSprite { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for MenuHandler class.
        /// </summary>
        public MenuHandler()
        {

        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.MENUHANDLER;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.MENUHANDLER;

            SwitchState(new StartState());
        }

        public void SwitchState(IState newState)
        {
            // Makes sure the state isn't null when exiting a state.
            // This is done to avoid an exception.
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            // "This" means the MenuHandler.
            currentState.Enter(this);
        }

        public void LoadGame()
        {
            Thread.Sleep(3000);

            //gameState = GameState.Playing;

            Loading = true;
        }

        #endregion
    }
}
