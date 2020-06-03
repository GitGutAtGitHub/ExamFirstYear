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

        private bool saveLoaded;

        private SpriteRenderer spriteRenderer;
        private Jump jump;

        #endregion


        #region Properties
        public int PlayerID { get; set; }
        public int InventoryID { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int OpenDoor { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public bool canAttack { get; set; } = true;
        public bool canShoot { get; set; } = true;
        public Movement Movement { get; private set; }
        public TmpJournal TmpJournal { get; private set; }
        public Vector2 Direction { get; set; } = new Vector2(1, 0);

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Player Character component.
        /// </summary>
        public Player(int playerID)
        {
            PlayerID = playerID;
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

            Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
            jump = (Jump)GameObject.GetComponent(Tag.JUMP);
            saveLoaded = true;
        }

        public override void Start()
        {
            saveLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            LoadSave();
            TestMethod();
        }

        #endregion


        #region Other methods

        public void Notify(GameEvent gameEvent, Component component)
        {
            //Players collect materials when they collide with them.
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MATERIAL)
            {
                Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);
                component.GameObject.Destroy();
                SQLiteHandler.Instance.IncreaseAmountStoredMaterial(componentMaterial.MaterialID);
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

                tmpMeleeObject.Transform.Position = GameObject.Transform.Position + (new Vector2(Direction.X * tmpMeleeRenderer.Sprite.Width, Direction.Y));

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

                tmpProjectileObject.Transform.Position = GameObject.Transform.Position;

                Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);

                tmpMovement.Velocity = Direction;

                //tmpMovement.Speed = 1000f;
                GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
                GameWorld.Instance.GameObjects.Add(tmpProjectileObject);

                canShoot = false;
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
