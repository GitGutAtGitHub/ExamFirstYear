using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	/// <summary>
	/// The Player Character class.
	/// </summary>
	public class Player : Component, IGameListener
	{
		#region Fields

		private MouseState previousMouseState;
		private MouseState currentMouseState;

		private bool saveLoaded;
		private float manaRegenerateTimer;
		private bool playerCollidingWithDoor = false;
		// First wait is sat high, as to ensure the mana doesn't regenerate until after a short while from when the last time they used mana.
		private float initialRegenerationTimer = 2.5f;
		// After the first wait, the timer is not sat to 0, so it takes less time for mana to regenerate once the regeneration has begun.
		private float regenerationTimer = 1.5f;
		private float invulnerabilityTimer;
		private float invulnerabilityFrames = 0.8f;
		private byte fullMana = 5;
		private bool canShoot = true;

		private SQLiteHandler sQLiteHandler = GameWorld.Instance.SQLiteHandler;
		private Journal journal = GameWorld.Instance.Journal;

		//private SpriteRenderer spriteRenderer;
		//private RangedAttack rangedAttack;
		//private Jump jump;
		//private Player player = GameWorld.Instance.player; Tror ikke vi behøver denne her

		#endregion


		#region Properties
		public int PlayerID { get; set; }
		public int InventoryID { get; set; }
		public int Health { get; set; }
		public int Mana { get; set; }
		public int OpenDoor { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public bool CanRegenerateMana { get; set; }
		public bool AllMaterialsCollected { get; set; }
		public bool PlayerCollidingWithDoor { get => playerCollidingWithDoor; set => playerCollidingWithDoor = value; }
        public bool SaveLoaded { get; set; }
        public Vector2 PlayerVelocity { get; set; } = new Vector2(1, 0);
		public SpriteRenderer SpriteRenderer { get; private set; }
		public Movement Movement { get; private set; }
		public Jump Jump { get; private set; }
		public RangedAttack RangedAttack { get; private set; }
		public AttackMelee AttackMelee { get; private set; }
		public SoundComponent SoundComponent { get; private set; }
		public LightSource LightSource { get; private set; }
		public AnimationHandler AnimationHandler { get; private set; }
		//public string[] WalkSpritesNames { get; } = new string[2] { "", "" };
		//public string[] AttackSpritesNames { get; } = new string[2] { "", "" };
		//public string[] JumpSpritesNames { get; } = new string[1] { " " };

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for the Player Character component.
		/// </summary>
		public Player(int playerID)
		{
			PlayerID = playerID;
		}

		//empty constructor used for unittesting
		public Player()
		{

		}
		#endregion


		#region Override methods

		public override Tag ToEnum()
		{
			return Tag.PLAYER;
		}

		public override void Awake()
		{
			GameObject.Tag = Tag.PLAYER;

			// Kan dette flyttes ned i start efter SoundComponent er sat? Hilsen Emma
			((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("footstepsLouder", false);
			((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("Whoosh m. reverb", false);
			((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("Jump_04", false);
			((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("RangedAttack3", false);

			GameObject.SpriteName = "OopPlayerSprite2";

            SaveLoaded = false;

            AllMaterialsCollected = false;
        }

		public override void Start()
		{
			SpriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
			Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
			Jump = (Jump)GameObject.GetComponent(Tag.JUMP);
			RangedAttack = (RangedAttack)GameObject.GetComponent(Tag.RANGEDATTACK);
			AttackMelee = (AttackMelee)GameObject.GetComponent(Tag.ATTACKMELEE);
			SoundComponent = (SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT);
			LightSource = (LightSource)GameObject.GetComponent(Tag.LIGHTSOURCE);
			AnimationHandler = (AnimationHandler)GameObject.GetComponent(Tag.ANIMATIONHANDLER);
			SoundComponent.Volume = 1;
			LoadSave();
		}

		public override void Update(GameTime gameTime)
		{
			TestMethod();
			RegenerateMana();
		}

		#endregion

		#region Other methods

		public void CallMeleeAttack()
		{
			AttackMelee.MeleeAttack(this.GameObject, Tag.PLAYERMELEEATTACK, PlayerVelocity);

		}
		public void CallRangedAttack()
		{
			if (Mana > 0 && canShoot == true)
			{
				RangedAttack.RangedAttackMethod(this.GameObject, Tag.PLAYERPROJECTILE, PlayerVelocity);
				if (RangedAttack.HasShot == true)
				{
					Mana--;
					CanRegenerateMana = false;
					ManagePlayerLight(-0.5f);
					canShoot = false;
				}
			}
		}

		/// <summary>
		/// Methods used when the ranged attack button is released. Sets CanShoot to true.
		/// </summary>
		public void PlayerReleaseRangedAttack()
		{
			canShoot = true;
		}

		/// <summary>
		/// Method used when the player takes damage.
		/// Invulnerability frames have been added so the player has a chance to get away
		/// before taking more damage.
		/// </summary>
		public void TakeDamage()
		{
			invulnerabilityTimer -= GameWorld.Instance.DeltaTime;

			if (invulnerabilityTimer <= 0)
			{
				Health--;
				//Console.WriteLine(Health);
				invulnerabilityTimer = invulnerabilityFrames;
			}
		}

		public void Notify(GameEvent gameEvent, Component component)
		{
			//Players collect materials when they collide with them.
			if (gameEvent.Title == "Colliding" && (component.GameObject.Tag == Tag.SPIDERFILAMENT || component.GameObject.Tag == Tag.MOTHWING || component.GameObject.Tag == Tag.MATCHHEAD))
			{
				Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);
				component.GameObject.Destroy();
				sQLiteHandler.IncreaseAmountStoredMaterial(componentMaterial.MaterialID, GameWorld.Instance.Inventory.InventoryID);
			}

			// Player looses health when colliding with an enemy.
			if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.FLYINGENEMY ||
				gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MEELEEENEMY ||
				gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.RANGEDENEMY ||
				gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.ENEMYPROJECTILE ||
				gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.ENEMYMELEEATTACK)
			{
				TakeDamage();
			}

			//Players hit platforms when they collide with them.
			if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLATFORM)
			{
				SpriteRenderer.SetSprite("OopPlayerSprite2");
				Rectangle intersection = Rectangle.Intersect(((Collider)(component.GameObject.GetComponent(Tag.COLLIDER))).CollisionBox,
										((Collider)(GameObject.GetComponent(Tag.COLLIDER))).CollisionBox);

				//Top and bottom platform.
				if (intersection.Width > intersection.Height)
				{
					//Top platform.
					if (component.GameObject.Transform.Position.Y > GameObject.Transform.Position.Y)
					{
						GameObject.Transform.Translate(new Vector2(0, -intersection.Height + 1));
					}

					//Bottom platform.
					if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
					{
						GameObject.Transform.Translate(new Vector2(0, +intersection.Height - 1));
					}
				}

				// Left and right platform.
				else if (intersection.Width < intersection.Height)
				{
					//Right platform.
					if (component.GameObject.Transform.Position.X < GameObject.Transform.Position.X)
					{
						GameObject.Transform.Translate(new Vector2(+intersection.Width, 0));
					}

					//Left platform.
					if ((component.GameObject.Transform.Position.X > GameObject.Transform.Position.X))
					{
						GameObject.Transform.Translate(new Vector2(-intersection.Width, 0));
					}
				}
			}
		}

        public void LoadSave()
        {
            if (SaveLoaded == false)
            {
                InventoryID = sQLiteHandler.SelectIntValuesWhere("InventoryID", "Journal",
                            $"ID={journal.JournalID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                Health = sQLiteHandler.SelectIntValuesWhere("Health", "Journal",
                            $"ID={journal.JournalID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                OpenDoor = sQLiteHandler.SelectIntValuesWhere("OpenDoor", "Journal",
                            $"ID={journal.JournalID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                Mana = sQLiteHandler.SelectIntValuesWhere("Mana", "Journal",
                            $"ID={journal.JournalID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                sQLiteHandler.ClearTable("StoredMaterial", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                sQLiteHandler.ClearTable("RecordedCreature", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                AllMaterialsCollected = false;
            }

            SaveLoaded = true;
        }

        /// <summary>
        /// Method used to regenerate mana when needed and under certain conditions.
        /// A timer is used to make sure mana regenerates over time. Once the first mana point has regenerated,
        /// the rest regenerates faster. However, if the player fires another ranged attack, the timer is resat to 0,
        /// and the regeneration will be slow to get the first next mana point back, and then mana starts regenerating faster once again.
        /// </summary>
        private void RegenerateMana()
		{
			// Once mana is lower than the full amount of mana, a timer starts and regeneration of mana can begin.
			if (Mana < fullMana)
			{
				manaRegenerateTimer += GameWorld.Instance.DeltaTime;

				// Once the manaRegenerateTimer reachers initialRegenerationTimer, mana is regenerated.
				if (manaRegenerateTimer >= initialRegenerationTimer)
				{
					// Adds one mana.
					Mana++;
					ManagePlayerLight(+0.5f);
					// Resets the timer.
					manaRegenerateTimer = regenerationTimer;
				}
			}

			// If the player shoots the timer resets to 0. This is to make the mana regeneration more smooth
			// and to ensure the player takes a break from using the ranged attack to let their mana regenerate.
			if (CanRegenerateMana == false)
			{
				manaRegenerateTimer = 0;

				// Sets the bool to true, so the timer manaRegenerator can start going up again.
				CanRegenerateMana = true;
			}

			if (Mana == fullMana)
			{
				// Once mana is full, the timer is resat to 0.
				manaRegenerateTimer = 0;
			}
		}

		private void ManagePlayerLight(float value)
		{
			LightSource.LightRadiusScale += value;
		}

        private bool CheckBlueprint(int blueprintID)
        {
            bool areEqual;


            int storedSpiderFilamentAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "StoredMaterial",
                                             $"InventoryID={GameWorld.Instance.Inventory.InventoryID} AND MaterialTypeID=1",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
            int requiredSpiderFilamentAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial",
                                             $"BlueprintID={blueprintID} AND MaterialTypeID=1",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

            int storedMatchHeadAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "StoredMaterial",
                                             $"InventoryID={GameWorld.Instance.Inventory.InventoryID} AND MaterialTypeID=2",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
            int requiredMatchHeadAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial",
                                             $"BlueprintID={blueprintID} AND MaterialTypeID=2",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

            int storedMothWingAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "StoredMaterial",
                                             $"InventoryID={GameWorld.Instance.Inventory.InventoryID} AND MaterialTypeID=3",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
            int requiredMothWingAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial",
                                             $"BlueprintID={blueprintID} AND MaterialTypeID=3",
                                               new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

            if (storedSpiderFilamentAmount >= requiredSpiderFilamentAmount &&
                storedMatchHeadAmount >= requiredMatchHeadAmount &&
                storedMothWingAmount >= requiredMothWingAmount)
            {
                areEqual = true;
            }

            else
            {
                areEqual = false;
            }

            return areEqual;


            //bool areEqual = false;

            //Dictionary<int, int> storedMaterials = new Dictionary<int, int>();
            //Dictionary<int, int> requiredMaterials = new Dictionary<int, int>();

            //List<int> storedMaterialTypeIDs = new List<int>(sQLiteHandler.ExecuteIntReader("MaterialTypeID", "StoredMaterial",
            //    $"InventoryID = {GameWorld.Instance.Inventory.InventoryID}"));

            //List<int> requiredMaterialTypeIDs = new List<int>(sQLiteHandler.ExecuteIntReader("MaterialTypeID", "RequiredMaterial",
            //        $"BlueprintID = {blueprintID}"));


            ////Fill out dictionary with stored materials.
            //foreach (int storedMaterialTypeID in storedMaterialTypeIDs)
            //{
            //    int storedMaterialAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "StoredMaterial", $"MaterialTypeID={storedMaterialTypeID} " +
            //                           $"AND InventoryID={GameWorld.Instance.Inventory.InventoryID}",
            //                           new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

            //    storedMaterials.Add(storedMaterialTypeID, storedMaterialAmount);
            //}

            ////Fill out dictionary with required materials.
            //foreach(int requiredMaterilTypeID in requiredMaterialTypeIDs)
            //{
            //    int requiredMaterialAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial", $"MaterialTypeID={requiredMaterilTypeID} " +
            //                            $"AND BlueprintID={blueprintID}",
            //                            new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

            //    requiredMaterials.Add(requiredMaterilTypeID, requiredMaterialAmount);
            //}


            //if (storedMaterials == requiredMaterials)
            //{
            //    areEqual = true;
            //}
        }

        #endregion

        public void TestMethod()
		{
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                //Health--;
            }

            else if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
			{
                sQLiteHandler.IncreaseAmountStoredMaterial(1, GameWorld.Instance.Inventory.InventoryID);
                sQLiteHandler.IncreaseAmountStoredMaterial(2, GameWorld.Instance.Inventory.InventoryID);
                sQLiteHandler.IncreaseAmountStoredMaterial(3, GameWorld.Instance.Inventory.InventoryID);

                if (CheckBlueprint(1) == true)
                {
                    AllMaterialsCollected = true;
                }
            }
		}
	}
}
