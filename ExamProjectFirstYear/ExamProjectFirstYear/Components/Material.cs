using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Material component class.
    /// </summary>
    public class Material : Component, IGameListener
    {
        #region Fields

        private Movement movement;

        #endregion


        #region Properties

        public int MaterialID { get; set; }

        #endregion


        #region Constructors
        
        /// <summary>
        /// Constructor for material.
        /// </summary>
        /// <param name="iD"></param>
        public Material(int iD)
        {
            MaterialID = iD;
        }

        #endregion


        #region Override methods

        public override Tag ToEnum()
        {
            return Tag.MATERIAL;
        }

		#endregion


		#region Other methods

		public void Notify(GameEvent gameEvent, Component component)
        {
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

        public Material Clone()
        {
            return (Material)this.MemberwiseClone();
        }

        #endregion
    }
}
