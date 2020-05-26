﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class Material : Component, IGameListener
    {
        #region Fields

        private Movement movement;

        #endregion


        #region Properties

        public MaterialType MaterialType { get; set; }

        #endregion


        #region Constructors

        public Material(MaterialType materialType)
        {
            MaterialType = materialType;
        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.MATERIAL;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.MATERIAL;
            GameObject.SpriteName = "OopBossProjectileSprite2";
        }

        public override void Start()
        {
            movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
            GameObject.Transform.Translate(new Vector2(500, 10));
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
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
                        movement.Grounded = true;
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

        #endregion
    }
}
