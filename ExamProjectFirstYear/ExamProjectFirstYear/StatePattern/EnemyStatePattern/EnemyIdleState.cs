using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
                        //(enemy as MeleeEnemy).SwitchState(new EnemyAttackState());
                        break;

                    case Tag.FLYINGENEMY:
                        (enemy as FlyingEnemy).SwitchState(new EnemyAttackState());
                        break;

                    case Tag.RANGEDENEMY:
                        //(enemy as RangedEnemy).SwitchState(new EnemyAttackState());
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

        }

        /// <summary>
        /// Puts the FlyingEnemy thread to sleep to stop it from moving after the player,
        /// when out of reach.
        /// </summary>
        public void IdleFlyingEnemy()
        {

            enemy.Velocity = new Vector2(0, 0);

        }

        /// <summary>
        ///
        /// </summary>
        public void IdleRangedEnemy()
        {

        }

        public void Exit()
        {

        }


    }
}
