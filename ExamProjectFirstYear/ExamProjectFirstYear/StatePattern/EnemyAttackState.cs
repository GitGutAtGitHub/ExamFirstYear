using ExamProjectFirstYear.Components;
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
                        MeleeEnemyAttack();
                        break;

                    case Tag.FLYINGENEMY:
                        FlyingEnemyAttack();
                        break;

                    case Tag.RANGEDENEMY:
                        RangedEnemyAttack();
                        break;
                }
            }

            // Used to change the state to idle when the player is out of the enemies SightRadius.
            else
            {
                switch (enemy.ToEnum())
                {
                    case Tag.MEELEEENEMY:
                        (enemy as MeleeEnemy).SwitchState(new EnemyIdleState());
                        break;

                    case Tag.FLYINGENEMY:
                        (enemy as FlyingEnemy).SwitchState(new EnemyIdleState());
                        break;

                    case Tag.RANGEDENEMY:
                        (enemy as RangedEnemy).SwitchState(new EnemyIdleState());
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
            enemy.FollowPath(true);

            if (true)
            {

            }
        }

        public void MeleeEnemyAttack()
        {
            

            enemy.GeneratePath();
            enemy.FollowPath(false);

        }

        public void RangedEnemyAttack()
        {
            enemy.Velocity = Vector2.Zero;
            enemy.Path.Clear();

            Console.WriteLine("BOOOM");
        }

        public void Exit()
        {

        }


        public Tag ToTag()
        {
            return Tag.ENEMYATTACKSTATE;
        }


    }

}
