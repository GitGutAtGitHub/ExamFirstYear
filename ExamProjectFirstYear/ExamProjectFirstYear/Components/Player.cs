﻿using ExamProjectFirstYear.Components;
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

        private int journalID;

        #endregion


        #region Properties

        public int Health { get; set; }
        public int JournalID { get; set; }
        public Movement Movement { get; private set; }
        public int JournalID1 { get => journalID; set => journalID = value; }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Player Character component.
        /// </summary>
        public Player(int journalID)
        {
            this.journalID = journalID;
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
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MATERIAL)
            {
                Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);

                switch (componentMaterial.MaterialType)
                {
                    case MaterialType.LightBulb:
                        component.GameObject.Destroy();
                        SQLiteHandler.Instance.IncreaseAmountStoredMaterial(1);
                        break;
                }
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



        #endregion
    }
}
