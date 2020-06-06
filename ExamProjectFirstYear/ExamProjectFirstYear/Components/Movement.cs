using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Component that enabled sprite movement.
    /// </summary>
    public class Movement : Component, IGameListener
    {
        #region Fields

        private bool gravityOn;
        private float force;
        private float speed;


        #endregion


        #region Properties

        public Vector2 Velocity { get; set; }
        public bool Grounded { get; set; }
        public float Force { get => force; set => force = value; }

        #endregion


        #region Contstructors

        /// <summary>
        /// Constructor for Movement component.
        /// </summary>
        /// <param name="gravityOn"></param>
        /// <param name="maxMomentum"></param>
        /// <param name="speed"></param>
        public Movement(bool gravityOn, float speed)
        {
            this.gravityOn = gravityOn;
            this.speed = speed;

        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.MOVEMENT;
        }

        public override void Update(GameTime gameTime)
        {
            if (gravityOn == true)
            {
                GravityHandling();
            }

            Move(Velocity);
        }

        /// <summary>
        /// Enables movement of the object.
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
        /// Applies gravity to an object that has left the ground.
        /// </summary>
        public void GravityHandling()
        {
            /// If the GameObjects position is above the bottom of the screen and Grounded is false (if the GameObject is not on a platform) it will be pulled
            /// down by the value of force.
            if (Grounded == false)
            {
                /// As long as force is a higher value than -20 it will be lowered.
                /// This ensures that force will not become so low that it can pull the GameObject through a platform in a single frame.
                if (force >= -20f)
                {
                    force--;
                }
                GameObject.Transform.Translate(new Vector2(0, -force));
            }
            if (Grounded == true)
            {
                force = 0;
            }
        }

        /// <summary>
        /// Mostly relevant for player. Notifies when the GameObject in question collides with a platform. Ensures that Grounded is set to true
        /// So that Gravityhandling will set the force to zero, so that when the GameObject (for example player) falls from a platform it isn't
        /// immediately pulled down with the maxforce of -20. Basically this imitates wind resistance.
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "NoLongerColliding")
            {
				if (component.GameObject.Tag == Tag.PLATFORM)
				{
                    Grounded = false;
                }
            }

            if (gameEvent.Title == "Colliding")
            {
                Rectangle intersection = Rectangle.Intersect(((Collider)(component.GameObject.GetComponent(Tag.COLLIDER))).CollisionBox,
                                    ((Collider)(GameObject.GetComponent(Tag.COLLIDER))).CollisionBox);

				if (component.GameObject.Tag == Tag.PLATFORM)
				{
                    //Top and bottom platform.
                    if (intersection.Width > intersection.Height)
                    {
                        //Top platform.
                        if (component.GameObject.Transform.Position.Y > GameObject.Transform.Position.Y)
                        {
                            Grounded = true;
                        }
                    }
                }
                // Ensures that GameObjects that aren't platforms can't set the value Grounded to true which would potentially stop the player midair.
				else
				{
					Grounded = false;
				}
            }
        }
        #endregion
    }
}
