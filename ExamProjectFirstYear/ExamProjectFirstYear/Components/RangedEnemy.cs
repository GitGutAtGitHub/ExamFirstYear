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
    class RangedEnemy : Enemy
    {

        public override void Awake()
        {
            SightRadius = 1 * NodeManager.Instance.CellSize;
            speed = 200f;
            GameObject.Tag = Tag.RANGEDENEMY;
            SwitchState(new EnemyIdleState());
        }

        public override void Start()
        {
            GameObject.SpriteName = "FlyingEnemy";
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
        }
        protected override void DropMaterialUponDeath()
        {
            throw new NotImplementedException();
        }

        protected override void EnemyDeath()
        {
            throw new NotImplementedException();
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

        protected override void Notify()
        {
            throw new NotImplementedException();
        }
        public override Tag ToEnum()
        {
            return Tag.RANGEDENEMY;
        }

    }
}
