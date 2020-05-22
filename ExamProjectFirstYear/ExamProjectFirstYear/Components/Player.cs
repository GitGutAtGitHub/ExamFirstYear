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

        private float speed;

        private float ScreenSizeX = GameWorld.Instance.ScreenSize.X;
        private float ScreenSizeY = GameWorld.Instance.ScreenSize.Y;

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
            speed = 500;
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
            GameObject.Transform.Position = new Vector2(ScreenSizeX / 2, ScreenSizeY / 2);
            GameObject.Tag = Tag.PLAYER;
            GameObject.SpriteName = "OopPlayerSprite2";
        }

        public override void Start()
        {

        }

        public void Notify(GameEvent gameEvent, Component component)
        {
          
        }

        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Makes the player move.
        /// </summary>
        /// <param name="velocity"></param>
        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;

            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
        }

        /// <summary>
        /// Players method for attacking.
        /// </summary>
        /// <param name="attackNumber"></param>
        public void Attack(int attackNumber)
        {
            switch(attackNumber)
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

        #endregion
    }
}
