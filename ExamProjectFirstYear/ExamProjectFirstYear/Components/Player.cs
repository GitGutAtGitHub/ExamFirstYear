using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
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

        private bool hasJumped;

        //private float screenSizeX = GameWorld.Instance.ScreenSize.X;
        //private float screenSizeY = GameWorld.Instance.ScreenSize.Y;
        //private float playerPositionY;

        private Movement movement;
        private Gravity gravity;
        private SpriteRenderer spriteRenderer;

        #endregion


        #region Properties

        public int Health { get; set; }
        public int JournalID { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Player Character component.
        /// </summary>
        public Player()
        {
            //Lav en ny instans af JournalDB her.
        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.PLAYER;
        }

        public override void Awake()
        {
            //Define start position here
            //GameObject.Transform.Position = new Vector2(ScreenSizeX / 2, ScreenSizeY / 2);

            GameObject.Tag = Tag.PLAYER;
            GameObject.SpriteName = "OopPlayerSprite2";
        }

        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
            gravity = (Gravity)GameObject.GetComponent(Tag.GRAVITY);
            movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);

            movement.Momentum = GameWorld.Instance.ScreenSize.Y / 28 /*35*/;
            movement.Speed = 500;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void Notify(GameEvent gameEvent, Component component)
        {

        }

        public void CommandMovement(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= movement.Speed;

            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
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
            //Insert melee attack here.
        }

        /// <summary>
        /// Ranged attack for Player.
        /// </summary>
        private void RangedAttack()
        {
            //Insert ranged attack here.
        }

        /// <summary>
        /// Enables jumping.
        /// </summary>
        public void Jump()
        {
            if (gravity.Grounded == true)
            {
                Gravity.Force = movement.Momentum;

                GameObject.Transform.Translate(new Vector2(0, -Gravity.Force));
            }

            /// If the player is standing on the bottom of the screen hasJumped is set to false so the player may jump again.
            if (GameObject.Transform.Position.Y >= GameWorld.Instance.ScreenSize.Y - spriteRenderer.Sprite.Height / 2)
            {
                hasJumped = false;
            }
        }

        #endregion
    }
}
