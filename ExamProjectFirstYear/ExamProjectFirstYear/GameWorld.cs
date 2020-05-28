using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.Components;

namespace ExamProjectFirstYear
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameWorld : Game
	{
		#region FIELDS
		private SpriteBatch spriteBatch;

		//For singleton pattern
		private static GameWorld instance;

		GraphicsDeviceManager graphics;

		public Player player;

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
		public float DeltaTime { get; set; }
		public float TimeElapsed { get; set; }
		public Vector2 ScreenSize { get; private set; }

		#endregion

		#region Constructors

		private GameWorld()
		{
			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferHeight = 1000,
				PreferredBackBufferWidth = 1500
			};
			graphics.ApplyChanges();

			Content.RootDirectory = "Content";
			GetScreenSize();
		}

        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
		{
			TimeElapsed = 0;

			player = new Player(1);

			graphics.PreferredBackBufferWidth = 1920;
			graphics.PreferredBackBufferHeight = 1080;
			graphics.ApplyChanges();
			IsMouseVisible = true;


			NodeManager.Instance.InitializeGrid();
			

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

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Awake();
			}

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Start();
			}

			LevelManager.Instance.InitializeLevel();
			NodeManager.Instance.UpdateGrid();
			CreateObject(Tag.MATERIAL);
			NodeManager.Instance.LoadContent(Content);
			
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
				Exit();

			TimeHandler(gameTime);

			InputHandler.Instance.Execute(player);

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Update(gameTime);
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

			//SQLiteHandler.Instance.TestMethod();
			player.TestMethod();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Draw(spriteBatch);
			}

			NodeManager.Instance.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Removes a GameObject from the list of all GameObjects, so that the garbage collector can pick it up.
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
		}

		/// <summary>
		/// Get the screen size of the monitor.
		/// </summary>
		/// <returns></returns>
		private Vector2 GetScreenSize()
		{
			ScreenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			return ScreenSize;
		}

		/// <summary>
		/// Method for creating component objects.
		/// </summary>
		/// <param name="tag"></param>
		public void CreateObject(Tag tag)
		{
			GameObject createdObject = new GameObject();
			SpriteRenderer spriteRenderer = new SpriteRenderer();
			Collider collider;
			Material material = new Material(1);
			Blueprint blueprint = new Blueprint(1);

			switch (tag)
			{
				case Tag.MATERIAL:
					createdObject.AddComponent(material);
					createdObject.AddComponent(new Movement(true, 40, 500));
					break;

				case Tag.BLUEPRINT:
					createdObject.AddComponent(blueprint);
					break;
			}

			createdObject.AddComponent(spriteRenderer);

			createdObject.Awake();
			createdObject.Start();

			if (tag == Tag.MATERIAL)
			{
				collider = new Collider(spriteRenderer, material) { CheckCollisionEvents = true };
			}

			else
			{
				collider = new Collider(spriteRenderer);
			}

			createdObject.AddComponent(collider);

			Colliders.Add(collider);
			GameObjects.Add(createdObject);
		}
	}
}
