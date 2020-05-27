using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    public class Projectile : Component, IGameListener
    {
        private float timer = 0.2f;
        private bool timeUp = false;

        public Projectile()
        {
        }

        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

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

        public override void Destroy()
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

        public void Notify(GameEvent gameEvent, Component other)
        {
            if (gameEvent.Title == "Colliding" && other.GameObject.Tag == Tag.PLATFORM)
            {
                GameObject.Destroy();
                //other.GameObject.Destroy();
            }
            if (gameEvent.Title == "Colliding" && other.GameObject.Tag == Tag.PLAYERMELEEATTACK)
            {
                GameObject.Destroy();
            }
        }
    }
}
