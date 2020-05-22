using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ExamProjectFirstYear
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameWorld : Game
	{
		#region FIELDS
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		//For singleton pattern
		private static GameWorld instance;

		private Player player;

		#endregion

		#region PROPERTIES
		//-----PROPERTIES-----

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
			graphics = new GraphicsDeviceManager(this);
			//{
			//	PreferredBackBufferWidth = GraphicsDevice.Viewport.Width,
			//	PreferredBackBufferHeight = GraphicsDevice.Viewport.Height
			//};
			//graphics.ApplyChanges();
			GetScreenSize();

			Content.RootDirectory = "Content";
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

			CreateObject(Tag.PLAYER);

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Awake();
			}

			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].Start();
			}			
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

			switch (tag)
			{
				case Tag.PLAYER:
					createdObject.AddComponent(player);
					break;
			}

			createdObject.AddComponent(spriteRenderer);
			createdObject.Awake();
			createdObject.Start();

			if (tag == Tag.PLAYER)
			{
				collider = new Collider(spriteRenderer, player) { CheckCollisionEvents = true };
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
