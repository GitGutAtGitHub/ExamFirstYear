using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// Component for projectiles.
    /// </summary>
    public class Projectile : Component, IGameListener
    {
        #region Fields

        private float timer = 0.1f;
        private bool timeUp = false;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for projectiles.
        /// </summary>
        public Projectile()
        {

        }

        #endregion


        #region Override methods

        public override void Update(GameTime gameTime)
        {
            // The following deletes a meleeattack when a selected timaspan is up
            timer -= GameWorld.Instance.DeltaTime;

            if (timer <= 0)
            {
                timeUp = true;
                timer = 0.1f;
            }
            if ((GameObject.Tag == Tag.PLAYERMELEEATTACK || GameObject.Tag == Tag.ENEMYMELEEATTACK) && timeUp)
            {
                GameObject.Destroy();
                timeUp = false;
            }
            if (OutOfBounds() == true)
			{
                GameObject.Destroy();
			}
        }

        private bool OutOfBounds()
		{
            Vector2 playerPosition = GameWorld.Instance.player.GameObject.Transform.Position;
            Vector2 position = GameObject.Transform.Position;

            if ((position.X - playerPosition.X < GameWorld.Instance.ScreenSize.width/2) &&
                    (playerPosition.X-position.X < GameWorld.Instance.ScreenSize.width/2) &&
                    (position.Y - playerPosition.Y < GameWorld.Instance.ScreenSize.height/2) &&
                    (playerPosition.Y - position.Y < GameWorld.Instance.ScreenSize.height/2))
			{
                return false;
			}
			else
			{
                return true;
			}
            
        }

        //public void MoveToObjectPool()
        //{
        //    //Use GameEvent and Notify, if (OnHitObject, OnPastBorders) then destroy collider
        //    switch (GameObject.Tag)
        //    {
        //        case Tag.BOSSPROJECTILE:
        //            BossProjectilePool.Instance.ReleaseObject(GameObject);
        //            break;
        //        case Tag.PLAYERMELEEATTACK:
        //            PlayerMeleeAttackPool.Instance.ReleaseObject(GameObject);
        //            break;
        //        case Tag.PLAYERPROJECTILE:
        //            PlayerProjectilePool.Instance.ReleaseObject(GameObject);
        //            break;
        //        case Tag.ENEMYPROJECTILE:
        //            EnemyProjectilePool.Instance.ReleaseObject(GameObject);
        //            break;
        //        case Tag.MELEEATTACK:
        //            MeleeAttackPool.Instance.ReleaseObject(GameObject);
        //            break;
        //    }
        //    GameWorld.Instance.Colliders.Remove((Collider)GameObject.GetComponent(Tag.COLLIDER));
        //}

        public override Tag ToEnum()
        {
            return Tag.PROJECTILE;
        }

        #endregion


        #region Other methods

        /// <summary>
        /// Create a projectile clone.
        /// </summary>
        /// <returns></returns>
        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

        public void Notify(GameEvent gameEvent, Component other)
        {
            //if (gameEvent.Title == "Colliding" && other.GameObject.Tag == Tag.PLATFORM && this.GameObject.Tag != Tag.ATTACKMELEE)
            //{
            //    MoveToObjectPool();
            //    GameObject.Destroy();
            //    //other.GameObject.Destroy();
            //}
            //if (gameEvent.Title == "Colliding" && other.GameObject.Tag == Tag.PLAYERMELEEATTACK)
            //{
            //    MoveToObjectPool();
            //    GameObject.Destroy();
            //}

            //if (gameEvent.Title == "Colliding" && this.GameObject.Tag == Tag.PLAYERPROJECTILE && other.GameObject.Tag == Tag.FLYINGENEMY)
            //{
            //    MoveToObjectPool();
            //    GameObject.Destroy();
            //}

            //if (gameEvent.Title == "Colliding" && this.GameObject.Tag == Tag.PLAYERPROJECTILE && other.GameObject.Tag == Tag.MEELEEENEMY)
            //{
            //    MoveToObjectPool();
            //    GameObject.Destroy();
            //}

            //if (gameEvent.Title == "Colliding" && this.GameObject.Tag == Tag.PLAYERPROJECTILE && other.GameObject.Tag == Tag.RANGEDENEMY)
            //{
            //    MoveToObjectPool();
            //    GameObject.Destroy();
            //}
        }

        #endregion
    }
}
