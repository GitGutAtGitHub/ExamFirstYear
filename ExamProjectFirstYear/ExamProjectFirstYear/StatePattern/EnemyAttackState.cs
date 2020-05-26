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
        //private bool CanFollowPlayer;


        public void Enter(IEntity enemy)
        {
            this.enemy = enemy as Enemy;
        }

        public void Execute()
        {
            // Picks which method to run depending on the enemy type.
            // Only runs when the player is within the enemy's SightRadius.
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
                        MeleeEnemyAttack();
                        break;

                    case Tag.FLYINGENEMY:
                        (enemy as FlyingEnemy).SwitchState(new EnemyIdleState());
                        (enemy as FlyingEnemy).CanFollowPlayer = false;
                        break;

                    case Tag.RANGEDENEMY:
                        RangedEnemyAttack();
                        break;
                }
            }
        }

        public void MeleeEnemyAttack()
        {

        }

        public void FlyingEnemyAttack()
        {
            (enemy as FlyingEnemy).CanFollowPlayer = true;
            
            if ((enemy as FlyingEnemy).CanFollowPlayer != false)
            {
                (enemy as FlyingEnemy).FlyingPath = (enemy as FlyingEnemy).EnemyPathFinder.FindPath
                 (enemy.GameObject.Transform.Position,
                 enemy.Target.Transform.Position);

                NodeManager.Instance.DebugPath.AddRange((enemy as FlyingEnemy).FlyingPath);

                // As long as there are more than 0 nodes on FlyingPath, it checks the first node in the stack.    
                if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                {
                    (enemy as FlyingEnemy).TargetPosition = (enemy as FlyingEnemy).FlyingPath.Peek().Position;


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

                    //if there is still path nodes, then pop the current
                    if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                    {
                        (enemy as FlyingEnemy).FlyingPath.Pop();
                    }
                }
                (enemy as FlyingEnemy).CanFollowPlayer = false;
            }
        }

        public void RangedEnemyAttack()
        {

        }

        public void Exit()
        {

        }
    }
}
