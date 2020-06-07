using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using ExamProjectFirstYear.PathFinding;
using System.Xml;
using System.Dynamic;
using System.Threading;
using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using Microsoft.Xna.Framework.Media;
using ExamProjectFirstYear.MenuStatePattern;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        #region FIELDS
        private SpriteBatch spriteBatch;
        private bool gameIsRunning = true;

        //For singleton pattern
        private static GameWorld instance;

        GraphicsDeviceManager graphics;

        public Player Player;
        public Journal Journal;
        public Inventory Inventory;
        private Camera camera;
        private Song bgAmbience;

        //Following used for the lighteffects.
        private Effect lightEffect;
        private RenderTarget2D mainTarget;
        private RenderTarget2D lightTarget;

        #endregion


        #region PROPERTIES

        //For singletong pattern
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }

                return instance;
            }
        }

        public List<GameObject> GameObjects { get; private set; } = new List<GameObject>();
        public List<Collider> Colliders { get; set; } = new List<Collider>();
        public List<LightSource> LightSources { get; set; } = new List<LightSource>();
        public float DeltaTime { get; set; }
        public float TimeElapsed { get; set; }
        public TimeSpan ElapsedGameTime { get; set; }
        public TwoDimensionalSize ScreenSize { get; private set; }
        public bool GameIsRunning { get => gameIsRunning; set => gameIsRunning = value; }
        public SpriteBatch SpriteBatch { get; set; }
        public SQLiteHandler SQLiteHandler { get; set; }


        #endregion


        #region CONSTRUCTORS

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 1080,
                PreferredBackBufferWidth = 1920
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            GetScreenSize();
        }

        #endregion


        #region Preset methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            SQLiteHandler = new SQLiteHandler();

            SQLiteHandler.BuildDatabase();

            MenuHandler.Instance.SwitchState(new LoadingState());

            TimeElapsed = 0;

            //Create instances of Player, Journal and Inventory.
            Journal = new Journal(1);
            Player = new Player(Journal.JournalID);
            Inventory = new Inventory(Player.PlayerID);

            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.ApplyChanges();
            IsMouseVisible = true;


            //camera = new Camera();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bgAmbience = Content.Load<Song>("Soundtrack_mixdown2");
            MediaPlayer.Play(bgAmbience);

            //startButton = Content.Load<Texture2D>("OopPlayerSprite2");
            //exitButton = Content.Load<Texture2D>("OopPlayerSprite2");

            //loadingScreen = Content.Load<Texture2D>("OopGameScreen");



            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Awake();
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Start();
            }

            CreateUIObject(Tag.JOURNAL);
            CreateUIObject(Tag.INVENTORY);
            CreateUIObject(Tag.PLAYERHEALTHUI);
            CreateUIObject(Tag.PLAYERMANAUI);

            LevelManager.Instance.InitializeLevel();

            NodeManager.Instance.InitializeGrid();
            NodeManager.Instance.UpdateGrid();
            NodeManager.Instance.LoadContent(Content);

            //Following is used for the light effect.
            lightEffect = Content.Load<Effect>("LightEffect");
            var pp = GraphicsDevice.PresentationParameters;
            mainTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            lightTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameIsRunning = false;

                Exit();
            }

            MenuHandler.Instance.CurrentState.Execute();

            if (MenuHandler.Instance.GameState == GameState.PlayingState && MenuHandler.Instance.GameState != GameState.PausedState)
            {
                TimeHandler(gameTime);

                InputHandler.Instance.Execute(Player);

                for (int i = 0; i < GameObjects.Count; i++)
                {
                    if (GameObjects[i].Components.ContainsKey(Tag.PLAYER) || GameObjects[i].Components.ContainsKey(Tag.JOURNAL)
                        || GameObjects[i].Components.ContainsKey(Tag.INVENTORY) || GameObjects[i].Components.ContainsKey(Tag.PLAYERHEALTHUI)
                        || GameObjects[i].Components.ContainsKey(Tag.PLAYERMANAUI))
                    {
                        GameObjects[i].Update(gameTime);
                    }


                    else if ((GameObjects[i].Transform.Position.X - Player.GameObject.Transform.Position.X) < (ScreenSize.width) &&
                        (Player.GameObject.Transform.Position.X - GameObjects[i].Transform.Position.X) < (ScreenSize.width) &&
                        (GameObjects[i].Transform.Position.Y - Player.GameObject.Transform.Position.Y) < (ScreenSize.height) &&
                        (Player.GameObject.Transform.Position.Y - GameObjects[i].Transform.Position.Y) < (ScreenSize.height))
                    {
                        GameObjects[i].Update(gameTime);
                    }

                }

                //Makes a copy of the collider list, to avoid any exception when removing from the collider list.
                //OnColliding and OnNoLongerColliding is run from here rather than an Update method in Colliders, as they need a parameter
                //for other Collider.


                // NOTE!!!!!!!! MÅSKE SKAL VI PRØVE AT SE OM VI KAN FÅ DET TIL AT KØRE I COLLIDERS UPDATE MED EN CHECKCOLLISION METODE KIG EVT.
                //PÅ DET SENERE - EMMA
                Collider[] tmpColliders = Colliders.ToArray();

                for (int i = 0; i < tmpColliders.Length; i++)
                {
                    for (int j = 0; j < tmpColliders.Length; j++)
                    {
                        tmpColliders[i].OnColliding(tmpColliders[j]);
                        tmpColliders[i].OnNoLongerColliding(tmpColliders[j]);
                    }
                }

                Camera.Instance.FollowPlayer(Player.GameObject);

                base.Update(gameTime);
            }
        }

        #endregion

        #region DRAW
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (MenuHandler.Instance.GameState == GameState.PlayingState && MenuHandler.Instance.GameState != GameState.PausedState)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.SetRenderTarget(lightTarget);

                DrawLightSourcesWithCameraCulling();

                GraphicsDevice.SetRenderTarget(mainTarget);
                GraphicsDevice.Clear(Color.BlanchedAlmond);

                DrawGameObjectsWithCameraCulling();

                GraphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                lightEffect.Parameters["lightMaskTexture"].SetValue(lightTarget);
                lightEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);

                spriteBatch.End();

                DrawUIObjects();

                base.Draw(gameTime);
            }
        }

        /// <summary>
        /// Draw the Ui Objects
        /// </summary>
        private void DrawUIObjects()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Camera.Instance.TransformCamera);
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //VI BØR MÅSKE LAVE EN LISTE AF UI OBJECTS I STEDET FOR AT HAVE DENNE LANGE IF SÆTNING
                if (GameObjects[i].Components.ContainsKey(Tag.JOURNAL)
                    || GameObjects[i].Components.ContainsKey(Tag.INVENTORY) || GameObjects[i].Components.ContainsKey(Tag.PLAYERHEALTHUI)
                    || GameObjects[i].Components.ContainsKey(Tag.PLAYERMANAUI))
                {
                    GameObjects[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// This method holds the code that is used to draw LightSources when they are within a certain range of player.
        /// </summary>
        private void DrawLightSourcesWithCameraCulling()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: Camera.Instance.TransformCamera);

            for (int i = 0; i < LightSources.Count; i++)
            {
                if ((LightSources[i].GameObject.Transform.Position.X - Player.GameObject.Transform.Position.X) < (ScreenSize.width) &&
                            (Player.GameObject.Transform.Position.X - LightSources[i].GameObject.Transform.Position.X) < (ScreenSize.width) &&
                            (LightSources[i].GameObject.Transform.Position.Y - Player.GameObject.Transform.Position.Y) < (ScreenSize.height) &&
                            (Player.GameObject.Transform.Position.Y - LightSources[i].GameObject.Transform.Position.Y) < (ScreenSize.height))
                {

                    LightSources[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// This method holds the code that is used to draw GameObjects when they are within a certain range of player.
        /// </summary>
        private void DrawGameObjectsWithCameraCulling()
        {
            spriteBatch.Begin(transformMatrix: Camera.Instance.TransformCamera);
            //NodeManager.Instance.Draw(spriteBatch);

            for (int i = 0; i < GameObjects.Count; i++)
            {
                // ER DET NØDVENDIGT MED DENNE HER? PLAYERS POSITION VIL JO ALTID SØRGE FOR AT PLAYER BLIVER TEGNET. KAN DOG GODT VÆRE DEN SKAL
                //BRUGES TIL UI OBJEKTER (JOURNALEN OG HEARTS).
                if (GameObjects[i].Components.ContainsKey(Tag.PLAYER))
                {
                    GameObjects[i].Draw(spriteBatch);
                }

                // If the GameObject is not player it will only be drawn when it is within a certain distance of player.
                else if ((GameObjects[i].Transform.Position.X - Player.GameObject.Transform.Position.X) < (ScreenSize.width / 2) &&
                                (Player.GameObject.Transform.Position.X - GameObjects[i].Transform.Position.X) < (ScreenSize.width) &&
                                (GameObjects[i].Transform.Position.Y - Player.GameObject.Transform.Position.Y) < (ScreenSize.height) &&
                                (Player.GameObject.Transform.Position.Y - GameObjects[i].Transform.Position.Y) < (ScreenSize.height))
                {
                    GameObjects[i].Draw(spriteBatch);
                }
                // MÅ DET HER SLETTES?

                //else if ((GameObjects[i].Transform.Position.X - player.GameObject.Transform.Position.X) < (200) &&
                //  (player.GameObject.Transform.Position.X - GameObjects[i].Transform.Position.X) < (200) &&
                //  (GameObjects[i].Transform.Position.Y - player.GameObject.Transform.Position.Y) < (200) &&
                //  (player.GameObject.Transform.Position.Y - GameObjects[i].Transform.Position.Y) < (200))
                //{
                //    GameObjects[i].Draw(spriteBatch);
                //}
            }
            // HVAD BRUGER VI DENNE HER TIL METODEN ER TOM
            //NodeManager.Instance.Draw(spriteBatch);

            spriteBatch.End();
        }

        #endregion


        #region Specific methods

        /// <summary>
        /// Removes a GameObject from the list of all GameObjects.
        /// </summary>
        /// <param name="gameObject"></param>
        public void DeleteGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        /// <summary>
        /// Handles the timer in-game.
        /// </summary>
        /// <param name="gameTime"></param>
        private void TimeHandler(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            TimeElapsed += DeltaTime;

            ElapsedGameTime = gameTime.ElapsedGameTime;
        }

        /// <summary>
        /// Get the screen size of the monitor.
        /// </summary>
        /// <returns></returns>
        private TwoDimensionalSize GetScreenSize()
        {
            ScreenSize = new TwoDimensionalSize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            return ScreenSize;
        }

        /// <summary>
        /// Method for creating UI component objects. Specifically Journal and Inventory.
        /// </summary>
        /// <param name="tag"></param>
        private void CreateUIObject(Tag tag)
        {
            GameObject createdObject = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();

            switch (tag)
            {
                case Tag.JOURNAL:
                    createdObject.AddComponent(Journal);
                    //SpriteLayer ensures that the text can later be drawn on top of the Journal sprite.
                    spriteRenderer.SpriteLayer = 0.9f;
                    break;

                case Tag.INVENTORY:
                    createdObject.AddComponent(Inventory);
                    spriteRenderer.SpriteLayer = 0.7f;
                    break;

                case Tag.PLAYERHEALTHUI:
                    PlayerHealthUI playerHealthUI = new PlayerHealthUI();
                    createdObject.AddComponent(playerHealthUI);
                    spriteRenderer.SpriteLayer = 0.6f;
                    break;

                case Tag.PLAYERMANAUI:
                    PlayerManaUI playerManaUI = new PlayerManaUI();
                    createdObject.AddComponent(playerManaUI);
                    spriteRenderer.SpriteLayer = 0.6f;
                    break;
            }

            createdObject.AddComponent(spriteRenderer);

            createdObject.Awake();
            createdObject.Start();

            GameObjects.Add(createdObject);
        }

        #endregion
    }
}
