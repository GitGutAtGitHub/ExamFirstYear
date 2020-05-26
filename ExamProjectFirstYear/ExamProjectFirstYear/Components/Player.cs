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



        #endregion


        #region Properties

        public int Health { get; set; }
        public int JournalID { get; set; }
        public Movement Movement { get; private set; }

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
            GameObject.Tag = Tag.PLAYER;
            GameObject.SpriteName = "OopPlayerSprite2";
        }

        public override void Start()
        {
            Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);

            Movement.Momentum = GameWorld.Instance.ScreenSize.Y / 28 /*35*/;
            Movement.Speed = 500;
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == Tag.PLATFORM)
            {

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
            //Insert melee attack here.
        }

        /// <summary>
        /// Ranged attack for Player.
        /// </summary>
        private void RangedAttack()
        {

        }

        

        #endregion
    }
}
