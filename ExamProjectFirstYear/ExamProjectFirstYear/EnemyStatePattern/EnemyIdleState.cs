using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.StatePattern
{
    class EnemyIdleState : IState
    {
        private Enemy enemy;

        public void Enter(IEntity enemy)
        {
            this.enemy = enemy as Enemy;
        }

        public void Execute()
        {
            if (enemy.Target.Transform.Position.X <= (enemy.GameObject.Transform.Position.X + enemy.SightRadius) &&
                enemy.Target.Transform.Position.X >= (enemy.GameObject.Transform.Position.X - enemy.SightRadius) &&
                enemy.Target.Transform.Position.Y <= (enemy.GameObject.Transform.Position.Y + enemy.SightRadius) &&
                enemy.Target.Transform.Position.Y >= (enemy.GameObject.Transform.Position.Y - enemy.SightRadius))
            {
                switch (enemy.ToEnum())
                {
                    case Tag.MEELEEENEMY:
                        (enemy as MeleeEnemy).SwitchState(new EnemyAttackState());
                        break;

                    case Tag.FLYINGENEMY:
                        (enemy as FlyingEnemy).SwitchState(new EnemyAttackState());
                        break;

                    case Tag.RANGEDENEMY:
                        (enemy as RangedEnemy).SwitchState(new EnemyAttackState());
                        break;
                }
            }

            else

            {
                switch (enemy.ToEnum())
                {
                    case Tag.MEELEEENEMY:
                        IdleMeleeEnemy();
                        break;

                    case Tag.FLYINGENEMY:
                        IdleFlyingEnemy();
                        break;

                    case Tag.RANGEDENEMY:
                        IdleRangedEnemy();
                        break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void IdleMeleeEnemy()
        {
            if (enemy.Path.Count == 0 || enemy.Path == null)
            {
                if (!PathFinder.Instance.EnemiesNeedingPath.Contains(enemy))
                {
                    // Adds the flying enemy to the list of enemies that need to find a path.
                    PathFinder.Instance.EnemiesNeedingPath.Enqueue(enemy);
                }
            }

            // ((Movement)enemy.GameObject.GetComponent(Tag.MOVEMENT)).Velocity = new Vector2(-1, 0);
            //enemy.Velocity = new Vector2(-1, 0);
            enemy.FollowPath(false);
        }

        /// <summary>
        /// Puts the FlyingEnemy thread to sleep to stop it from moving after the player,
        /// when out of reach.
        /// </summary>
        public void IdleFlyingEnemy()
        {
            ((SoundComponent)enemy.GameObject.GetComponent(Tag.SOUNDCOMPONENT)).StopPlayingSound("MothMove");
            enemy.Velocity = new Vector2(0, 0);

        }

        /// <summary>
        ///
        /// </summary>
        public void IdleRangedEnemy()
        {
            if (enemy.Path.Count == 0 || enemy.Path == null)
            {
                if (!PathFinder.Instance.EnemiesNeedingPath.Contains(enemy))
                {
                    // Adds the flying enemy to the list of enemies that need to find a path.
                    PathFinder.Instance.EnemiesNeedingPath.Enqueue(enemy);
                }
            }

            // ((Movement)enemy.GameObject.GetComponent(Tag.MOVEMENT)).Velocity = new Vector2(-1, 0);
            //enemy.Velocity = new Vector2(-1, 0);
            enemy.FollowPath(false);

        }

        public void Exit()
        {

        }

        public Tag ToTag()
        {
            return Tag.ENEMYIDLESTATE;
        }
    }
}
