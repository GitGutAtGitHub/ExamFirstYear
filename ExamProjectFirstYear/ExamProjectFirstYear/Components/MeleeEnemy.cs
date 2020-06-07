using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// public for unit testing
    /// </summary>
    public class MeleeEnemy : Enemy, IGameListener
    {
        public override void Awake()
        {
            SightRadius = 5 * NodeManager.Instance.CellSize;
            health = 2;
            speed = 200f;
            GameObject.Tag = Tag.MEELEEENEMY;
            SwitchState(new EnemyIdleState());
            //((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound("footstepsLouder");

            ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("SpiderFootsteps_01", true);
            ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("Whoosh m. reverb", false);
            ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound2("SpiderDeath", false);
        }

        public override void Start()
        {
            GameObject.SpriteName = "FlyingEnemy";
            //((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).StartPlayingSound("footstepsLouder");
        }

        public override void AddTarget()
        {
            Target = GameWorld.Instance.player.GameObject;
        }



        public override void SwitchState(IState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = newState;
            // "This" means the FlyingEnemy.
            CurrentState.Enter(this);
        }

        public override void Update(GameTime gameTime)
        {
            CurrentState.Execute();
            Move();
            EnemyDeath();
        }

        protected override void Move()
        {
            
            if (Velocity != Vector2.Zero)
            {
                Velocity.Normalize();
            }

            Velocity *= speed;

            GameObject.Transform.Translate(Velocity * GameWorld.Instance.DeltaTime);
        }

        public override Tag ToEnum()
        {
            return Tag.MEELEEENEMY;
        }

        protected override void EnemyDeath()
        {
            if (health <= 0)
            {
                ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).StartPlayingSoundEffect("SpiderDeath");
                GameObject.Destroy();
                // 1 is the material ID for ?  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                DropMaterialUponDeath(1);
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            // If the enemy is hit by players projectile from the ranged attack or the melee attack,
            // the projectile is removed from the game and enemy looses 1 hp.
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLAYERPROJECTILE ||
                gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLAYERMELEEATTACK)
            {
                component.GameObject.Destroy();
                health--;
            }

            //Players collect materials when they collide with them.
            else if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.MATERIAL)
            {
                Material componentMaterial = (Material)component.GameObject.GetComponent(Tag.MATERIAL);
                component.GameObject.Destroy();
                SQLiteHandler.Instance.IncreaseAmountStoredMaterial(componentMaterial.MaterialID);
            }

            //Players hit platforms when they collide with them.
            else if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLATFORM)
            {
                Rectangle intersection = Rectangle.Intersect(((Collider)(component.GameObject.GetComponent(Tag.COLLIDER))).CollisionBox,
                                        ((Collider)(GameObject.GetComponent(Tag.COLLIDER))).CollisionBox);

                //Top and bottom platform.
                if (intersection.Width > intersection.Height)
                {
                    //Top platform.
                    if (component.GameObject.Transform.Position.Y > GameObject.Transform.Position.Y)
                    {
                        GameObject.Transform.Translate(new Vector2(0, -intersection.Height + 2));
                    }

                    //Bottom platform.
                    if (component.GameObject.Transform.Position.Y < GameObject.Transform.Position.Y)
                    {
                        GameObject.Transform.Translate(new Vector2(0, +intersection.Height - 2));
                    }
                }

                //// Left and right platform.
                //else if (intersection.Width < intersection.Height)
                //{
                //    //Right platform.
                //    if (component.GameObject.Transform.Position.X < GameObject.Transform.Position.X)
                //    {
                //        GameObject.Transform.Translate(new Vector2(+intersection.Width, 0));
                //    }

                //    //Left platform.
                //    if ((component.GameObject.Transform.Position.X > GameObject.Transform.Position.X))
                //    {
                //        GameObject.Transform.Translate(new Vector2(-intersection.Width, 0));
                //    }
                //}
            }
        }
        
    }
}
