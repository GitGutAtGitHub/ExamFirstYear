using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion


        #region Properties
        public int JournalID { get; set; }
        public int InventoryID { get; set; }
        public int Health { get; set; }
        public int OpenDoor { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public Movement Movement { get; private set; }

        public TmpJournal TmpJournal { get; private set; }

        public Vector2 Direction { get; set; } = new Vector2(1, 0);

    		public bool canAttack { get; set; } = true;
    		public bool canShoot { get; set; } = true;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Player Character component.
        /// </summary>
        public Player(int journalID)
        {
            JournalID = journalID;
        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.PLAYER;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.PLAYER;
            GameObject.SpriteName = "OopPlayerSprite2";
            TmpJournal = SQLiteHandler.Instance.GetJournal(JournalID);
        }

        public override void Start()
        {
            Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);

            GameObject.Transform.Translate(new Vector2(TmpJournal.TmpPositionX, TmpJournal.TmpPositionY));
            InventoryID = TmpJournal.TmpInventoryID;
            Health = TmpJournal.TmpHealth;
            OpenDoor = TmpJournal.TmpOpenDoor;
        }

        public override void Update(GameTime gameTime)
        {
            PositionX = GameObject.Transform.Position.X;
            PositionY = GameObject.Transform.Position.Y;
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MATERIAL)
            {
                Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);

                component.GameObject.Destroy();
                SQLiteHandler.Instance.IncreaseAmountStoredMaterial(componentMaterial.ID);
            }

            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLATFORM)
            {
                Rectangle intersection = Rectangle.Intersect(((Collider)(component.GameObject.GetComponent(Tag.COLLIDER))).CollisionBox,
                                        ((Collider)(GameObject.GetComponent(Tag.COLLIDER))).CollisionBox);

                //Top and bottom platform.
                if (intersection.Width > intersection.Height)
                {
                    //Top platform.
                    if (component.GameObject.Transform.Position.Y > GameObject.Transform.Position.Y)
                    {
                        GameObject.Transform.Translate(new Vector2(0, -intersection.Height));
                        Movement.Grounded = true;
                    }

                    //Bottom platform.
                    if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
                    {
                        GameObject.Transform.Translate(new Vector2(0, +intersection.Height));
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

        public void ReleaseAttack(int attackNumber)
    		{
    			if (attackNumber == 1)
    			{
    				canAttack = true;
    			}
    			if (attackNumber == 2)
    			{
    				canShoot = true;
    			}
    		}

        /// <summary>
        /// Players method for attacking.
        /// </summary>
        /// <param name="attackNumber"></param>
        public void Attack(int attackNumber)
        {
            switch (attackNumber)
            {
                case 1:
                    MeleeAttak();
                    break;

                case 2:
                    RangedAttack();
                    break;
            }
        }

        /// <summary>
    		/// Melee attack for Player.
    		/// </summary>
    		private void MeleeAttak()
    		{
    			if (canAttack)
    			{
    				GameObject tmpMeleeObject = PlayerMeleeAttackPool.Instance.GetObject();
    				SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
    				Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);
    				tmpMeleeObject.Transform.Position = this.GameObject.Transform.Position + (new Vector2(Direction.X * tmpMeleeRenderer.Sprite.Width, Direction.Y));
    				GameWorld.Instance.GameObjects.Add(tmpMeleeObject);
    				GameWorld.Instance.Colliders.Add(tmpMeleeCollider);
    				canAttack = false;
    			}
    		}

        /// <summary>
    		/// Ranged attack for Player.
    		/// </summary>
    		private void RangedAttack()
    		{
    			//MANGLER KODE DER FORHINDRER AT MAN KAN LAVE MERE END ÉT ANGREB AF GANGEN
    			//MANGLER OGSÅ KODE DER SØRGER FOR AT SPILLEREN MISTER LYS/MANA.
    			if (canShoot)
    			{
    				GameObject tmpProjectileObject = PlayerProjectilePool.Instance.GetObject();
    				Collider tmpProjectileCollider = (Collider)tmpProjectileObject.GetComponent(Tag.COLLIDER);
    				tmpProjectileObject.Transform.Position = this.GameObject.Transform.Position;
    				Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);
    				tmpMovement.Velocity = Direction;
    				//tmpMovement.Speed = 1000f;
    				GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
    				GameWorld.Instance.GameObjects.Add(tmpProjectileObject);
    				canShoot = false;
    			}
    		}

        public void ShowStoredMaterial(int materialTypeID, int inventoryID)
        {

        }

        #endregion

        public void TestMethod()
        {
            Blueprint blueprint = new Blueprint();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("Button pressed");

                //blueprint.CheckRecordedBP(1);

                //SQLiteHandler.Instance.SaveGame(Health, OpenDoor, PositionX, PositionY, JournalID);
            }
        }
    }

    public struct TmpJournal
    {
        public int TmpJournalID { get; set; }
        public int TmpInventoryID { get; set; }
        public int TmpHealth { get; set; }
        public int TmpOpenDoor { get; set; }
        public float TmpPositionX { get; set; }
        public float TmpPositionY { get; set; }


        public TmpJournal(int tmpJournalID, int tmpInventoryID, int tmpHealth, float tmpPositionX, float tmpPositionY, int tmpOpenDoor)
        {
            TmpJournalID = tmpJournalID;
            TmpInventoryID = tmpInventoryID;
            TmpHealth = tmpHealth;
            TmpPositionX = tmpPositionX;
            TmpPositionY = tmpPositionY;
            TmpOpenDoor = tmpOpenDoor;
        }
    }

    public struct TmpStoredMaterial
    {
        public int TmpAmount { get; set; }
        public int TmpSlot { get; set; }


        public TmpStoredMaterial(int tmpAmound, int tmpSlot)
        {
            TmpAmount = tmpAmound;
            TmpSlot = tmpSlot;
        }
    }
}
