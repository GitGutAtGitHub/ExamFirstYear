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
    class EnemyAttackState : IEnemyState
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
            // Find new path if target (players) position has changed since it last generated a path.
            if ((enemy as FlyingEnemy).PrevTargetNode !=
                new Vector2((int)(enemy.Target.Transform.Position.X / NodeManager.Instance.CellSize),
                            (int)(enemy.Target.Transform.Position.Y / NodeManager.Instance.CellSize)))
            {
             
                // Adds the flying enemy to the list of enemies that need to find a path.
                PathFinder.Instance.EnemiesNeedingPath.Enqueue(enemy);
            }

            if ((enemy as FlyingEnemy).FlyingPath != null)
            {
                // To avoid null exception
                if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                {
                    (enemy as FlyingEnemy).TargetPosition = new Vector2(((enemy as FlyingEnemy).FlyingPath.Peek().Position.X + NodeManager.Instance.CellSize / 2),
                                                                        ((enemy as FlyingEnemy).FlyingPath.Peek().Position.Y + NodeManager.Instance.CellSize / 2));

                    //calculating the direction-vector between the enemy and its target position
                    float vectorX = ((enemy as FlyingEnemy).TargetPosition.X) - (enemy.GameObject.Transform.Position.X);
                    float vectorY = ((enemy as FlyingEnemy).TargetPosition.Y) - (enemy.GameObject.Transform.Position.Y);

                    enemy.Velocity = new Vector2(vectorX, vectorY);

                    //checking the distance between the enemy and the targetposition.
                    dstX = Math.Abs(enemy.GameObject.Transform.Position.X - (enemy as FlyingEnemy).TargetPosition.X);
                    dstY = Math.Abs(enemy.GameObject.Transform.Position.Y - (enemy as FlyingEnemy).TargetPosition.Y);

                    //it has reached the end of the node, and is ready to get instructions for the next node.
                    if (dstX < 8 && dstY < 8)
                    {
                        //if there is still path nodes, then pop the current
                        if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                        {
                            (enemy as FlyingEnemy).FlyingPath.Pop();
                            enemy.Velocity = new Vector2(0, 0);
                        }
                    }
                }
            }
        }

        public void Exit()
        {

        }





    }

}
