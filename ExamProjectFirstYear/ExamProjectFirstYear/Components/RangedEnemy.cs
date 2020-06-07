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
    class RangedEnemy : Enemy, IGameListener
    {

        public override void Awake()
        {
            SightRadius = 5 * NodeManager.Instance.CellSize;
            speed = 200f;
            health = 1;
            GameObject.Tag = Tag.RANGEDENEMY;
            SwitchState(new EnemyIdleState());
            enemyID = 2;
        }

        public override void Start()
        {
            GameObject.SpriteName = "OopBossProjectileSprite2";
        }

        public override void AddTarget()
        {
            Target = GameWorld.Instance.Player.GameObject;
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

        protected override void EnemyDeath()
        {
            if (health <= 0)
            {
                GameObject.Destroy();
                
                DropMaterialUponDeath(Tag.MATCHHEAD);

                GameWorld.Instance.SQLiteHandler.AddRecordedCreature(enemyID, GameWorld.Instance.Journal.JournalID);
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
        }
        //protected override void Notify()
        //{
        //    throw new NotImplementedException();
        //}
        public override Tag ToEnum()
        {
            return Tag.RANGEDENEMY;
        }

    }
}
