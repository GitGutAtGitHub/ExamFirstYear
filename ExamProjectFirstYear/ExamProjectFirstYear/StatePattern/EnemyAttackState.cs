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
        private bool findNewPath = true;


        public void Enter(IEntity enemy)
        {
            this.enemy = enemy as Enemy;
        }

        public void Execute()
        {
            //Checks if the target is within range
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
                // call idle state.
            }
        }

        public void FlyingEnemyAttack()
        {


            //find new path
            if ((enemy as FlyingEnemy).PrevTargetNode !=
                new Vector2((int)(enemy.Target.Transform.Position.X / NodeManager.Instance.CellSize),
                            (int)(enemy.Target.Transform.Position.Y / NodeManager.Instance.CellSize)) && findNewPath == true)
            {
                (enemy as FlyingEnemy).FlyingPath = (enemy as FlyingEnemy).EnemyPathFinder.FindPath
                                                    (enemy.GameObject.Transform.Position,
                                                     enemy.Target.Transform.Position);

                //saves the previous target node, used to prevent astar calculating for the same targetposition
                (enemy as FlyingEnemy).PrevTargetNode = new Vector2(
                    (int)enemy.Target.Transform.Position.X / NodeManager.Instance.CellSize,
                    (int)enemy.Target.Transform.Position.Y / NodeManager.Instance.CellSize);
            }
        }


            //to avoid null exception
            if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                {
                    (enemy as FlyingEnemy).TargetPosition = new Vector2(((enemy as FlyingEnemy).FlyingPath.Peek().Position.X + NodeManager.Instance.CellSize / 2),
                                                                        ((enemy as FlyingEnemy).FlyingPath.Peek().Position.Y + NodeManager.Instance.CellSize / 2));

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

                NodeManager.Instance.DebugPath.AddRange((enemy as FlyingEnemy).FlyingPath);

                //checking the distance between the enemy and the targetposition.
                dstX = Math.Abs(enemy.GameObject.Transform.Position.X - (enemy as FlyingEnemy).TargetPosition.X);
                dstY = Math.Abs(enemy.GameObject.Transform.Position.Y - (enemy as FlyingEnemy).TargetPosition.Y);


                //when its nearly done with the current node, it is able to find a new path, if the player has changed its position.
                //this is done, so the enemy cant make any illegal moves, when changing direction midway through a cell/node.
                if (dstX < 4 && dstY < 4)
                {

                    if ((enemy as FlyingEnemy).PrevTargetNode !=
                        new Vector2((int)(enemy.Target.Transform.Position.X / NodeManager.Instance.CellSize),
                                    (int)(enemy.Target.Transform.Position.Y / NodeManager.Instance.CellSize)))
                    {
                        findNewPath = true;
                    }

                }

                else
                {
                    findNewPath = false;
                }

                //it has reached the end of the node, and is ready to get instructions for the next node.

                if (dstX < 2 && dstY < 2)
                {
                    enemy.Velocity = new Vector2(0, 0);

                    //if there is still path nodes, then pop the current
                    if ((enemy as FlyingEnemy).FlyingPath.Count > 0)
                    {
                        (enemy as FlyingEnemy).FlyingPath.Pop();
                    }
                }

            }
            else
            {
                findNewPath = true;
            }

        }

        public void Exit()
        {

        }
    }
}
