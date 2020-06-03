using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.GameObjects.Enemy;
using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.StatePattern
{
    class EnemyAttackState : IState
    {
        private Enemy enemy;
        private float dstX;
        private float dstY;

        public void Enter(IEntity enemy)
        {
            this.enemy = enemy as Enemy;
        }

        public void Execute()
        {
            //Checks if the target is within range. If the target is within range, an attack method is called.
            //If target is not within range, the enemy switches into their idle state.
            if (enemy.Target.Transform.Position.X <= (enemy.GameObject.Transform.Position.X + enemy.SightRadius) &&
               enemy.Target.Transform.Position.X >= (enemy.GameObject.Transform.Position.X - enemy.SightRadius) &&
               enemy.Target.Transform.Position.Y <= (enemy.GameObject.Transform.Position.Y + enemy.SightRadius) &&
               enemy.Target.Transform.Position.Y >= (enemy.GameObject.Transform.Position.Y - enemy.SightRadius))
            {
                switch (enemy.ToEnum())
                {
                    case Tag.MEELEEENEMY:
                        break;

                    case Tag.FLYINGENEMY:
                        FlyingEnemyAttack();
                        break;

                    case Tag.RANGEDENEMY:
                        break;
                }
            }

            // Used to change the state to idle when the player is out of the enemies SightRadius.
            else
            {
                switch (enemy.ToEnum())
                {
                    case Tag.MEELEEENEMY:

                        break;

                    case Tag.FLYINGENEMY:
                        (enemy as FlyingEnemy).SwitchState(new EnemyIdleState());
                        break;

                    case Tag.RANGEDENEMY:
                        break;
                }
            }
        }

        /// <summary>
        /// Runs the flying enemy's attack.
        /// </summary>
        public void FlyingEnemyAttack()
        {
            enemy.GeneratePath();
            enemy.FollowPath();
        }

        public void MeleeEnemyAttack()
        {
            enemy.Path.Clear();

            enemy.GeneratePath();
            enemy.FollowPath();

        }

        public void RangedEnemyAttack()
        {

        }

        public void Exit()
        {

        }





    }

}
