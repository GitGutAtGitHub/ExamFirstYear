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

                        break;

                    case Tag.FLYINGENEMY:
                        FlyingEnemyAttack();

                        break;

                    case Tag.RANGEDENEMY:
                        break;
                }
            }
            else
            {
                // call idle state.
            }
        }

        public void FlyingEnemyAttack()
        {
            
            (enemy as FlyingEnemy).FlyingPath = (enemy as FlyingEnemy).EnemyPathFinder.FindPath(enemy.GameObject.Transform.Position, enemy.Target.Transform.Position);
            
            NodeManager.Instance.DebugPath.AddRange((enemy as FlyingEnemy).FlyingPath);

            // As long as there are more than 0 nodes on FlyingPath, it checks the first node in the stack.
            if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
            {
                (enemy as FlyingEnemy).TargetPosition = (enemy as FlyingEnemy).FlyingPath.Peek().Position;
            }

            // These if-statements makes sure the enemy moves in the intended direction,
            // depending on where the target is.
            if (enemy.GameObject.Transform.Position.X >= (enemy as FlyingEnemy).TargetPosition.X &&
                enemy.GameObject.Transform.Position.X >= (enemy as FlyingEnemy).TargetPosition.X)
            {
                enemy.Velocity.X = -1f;
            }

            if (enemy.GameObject.Transform.Position.X <= (enemy as FlyingEnemy).TargetPosition.X &&
                enemy.GameObject.Transform.Position.X <= (enemy as FlyingEnemy).TargetPosition.X)
            {
                enemy.Velocity.X = 1f;
            }

            if (enemy.GameObject.Transform.Position.Y >= (enemy as FlyingEnemy).TargetPosition.Y &&
                enemy.GameObject.Transform.Position.Y >= (enemy as FlyingEnemy).TargetPosition.Y)
            {
                enemy.Velocity.Y = -1f;
            }

            if (enemy.GameObject.Transform.Position.Y <= (enemy as FlyingEnemy).TargetPosition.Y &&
                enemy.GameObject.Transform.Position.Y <= (enemy as FlyingEnemy).TargetPosition.Y)
            {
                enemy.Velocity.Y = 1f;
            }

            if (enemy.Velocity != Vector2.Zero)
            {
                // Ensures that the enemy doesn't move faster when it moves diagonally.
                enemy.Velocity.Normalize();
            }


            //if (Math.Abs(enemy.GameObject.Transform.Position.X - (enemy as FlyingEnemy).TargetPosition.X) > 1 &&
            //    Math.Abs(enemy.GameObject.Transform.Position.Y - (enemy as FlyingEnemy).TargetPosition.Y) > 1)
            //{

            //}
        }

        public void Exit()
        {

        }
    }
}
