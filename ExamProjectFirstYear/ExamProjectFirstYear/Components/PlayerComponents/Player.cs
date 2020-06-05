using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
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

        private bool saveLoaded;
        private float manaRegenerateTimer;
        // First wait is sat high, as to ensure the mana doesn't regenerate until after a short while from when the last time they used mana.
        private float initialRegenerationTimer = 2.5f;
        // After the first wait, the timer is not sat to 0, so it takes less time for mana to regenerate once the regeneration has begun.
        private float regenerationTimer = 1.5f;
        private float invulnerabilityTimer;
        private float invulnerabilityFrames = 0.8f;
        private byte fullMana = 5;

        private SpriteRenderer spriteRenderer;
        private RangedAttack rangedAttack;
        private Jump jump;
        private Player player = GameWorld.Instance.player;

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
        public Vector2 Velocity { get; set; } = new Vector2(1, 0);
        public Movement Movement { get; private set; }
        public TmpJournal TmpJournal { get; private set; }

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
            GameObject.SpriteName = "OopPlayerSprite2";

            TmpJournal = SQLiteHandler.Instance.GetJournal(PlayerID);

            saveLoaded = true;
        }

        public override void Start()
        {
            //Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
            //spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
            //jump = (Jump)GameObject.GetComponent(Tag.JUMP);

            saveLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            LoadSave();
            TestMethod();
            RegenerateMana();
        }

        #endregion


        #region Other methods

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
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MATERIAL)
            {
                Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);
                component.GameObject.Destroy();
                SQLiteHandler.Instance.IncreaseAmountStoredMaterial(componentMaterial.MaterialID);
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

        private void LoadSave()
        {
            if (saveLoaded == true)
            {
                //GameObject.Transform.Translate(new Vector2(TmpJournal.TmpPositionX - spriteRenderer.Sprite.Width * 3, TmpJournal.TmpPositionY - spriteRenderer.Sprite.Height));
                InventoryID = TmpJournal.TmpInventoryID;
                Health = TmpJournal.TmpHealth;
                OpenDoor = TmpJournal.TmpOpenDoor;
                Mana = TmpJournal.TmpMana;
            }

            saveLoaded = false;
        }

        /// <summary>
        /// Method used to regenerate mana when needed and under certain conditions.
        /// A timer is used to make sure mana regenerates over time. Once the first mana point has regenerated,
        /// the rest regenerates faster. However, if the player fires another ranged attack, the timer is resat to 0,
        /// and the regeneration will be slow to get the first next mana point back, and then mana starts regenerating faster once again.
        /// </summary>
        private void RegenerateMana()
        {
            //Console.WriteLine(manaRegenerateTimer);

            // Once mana is lower than the full amount of mana, a timer starts and regeneration of mana can begin.
            if (Mana < fullMana)
            {
                manaRegenerateTimer += GameWorld.Instance.DeltaTime;

                // Once the manaRegenerateTimer reachers initialRegenerationTimer, mana is regenerated.
                if (manaRegenerateTimer >= initialRegenerationTimer)
                {
                    // Adds one mana.
                    Mana++;
                    
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

        #endregion

        public void TestMethod()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Health--;
            }

            else if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                Mana--;
            }
        }
    }
}
