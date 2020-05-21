using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

		#endregion

		private GameWorld()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

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

			// TODO: use this.Content to load your game content here
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

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

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
	}
}
