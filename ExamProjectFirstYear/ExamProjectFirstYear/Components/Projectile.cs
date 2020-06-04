using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        private float timer = 0.5f;
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
                timer = 0.5f;
            }
            if (GameObject.Tag == Tag.PLAYERMELEEATTACK && timeUp)
            {
                GameObject.Destroy();
                timeUp = false;
            }

        }

        public void MoveToObjectPool()
        {
            //Use GameEvent and Notify, if (OnHitObject, OnPastBorders) then destroy collider
            switch (GameObject.Tag)
            {
                case Tag.BOSSPROJECTILE:
                    BossProjectilePool.Instance.ReleaseObject(GameObject);
                    break;
                case Tag.PLAYERMELEEATTACK:
                    PlayerMeleeAttackPool.Instance.ReleaseObject(GameObject);
                    break;
                case Tag.PLAYERPROJECTILE:
                    PlayerProjectilePool.Instance.ReleaseObject(GameObject);
                    break;
                case Tag.ENEMYPROJECTILE:
                    EnemyProjectilePool.Instance.ReleaseObject(GameObject);
                    break;
            }
            GameWorld.Instance.Colliders.Remove((Collider)GameObject.GetComponent(Tag.COLLIDER));
        }

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
