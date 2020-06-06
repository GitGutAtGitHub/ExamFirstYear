using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.StatePattern
{
    /// <summary>
    /// public for unit testing
    /// </summary>
    public class EnemyAttackState : IState
    {
        private Enemy enemy;
        private float dstX;
        private float dstY;

        TimeSpan cooldownTimer = new TimeSpan(0,0,0,0,500);

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
            if (enemy.Path.Count <= 2)
            {
                if (((AttackMelee)enemy.GameObject.GetComponent(Tag.ATTACKMELEE)).canAttack == true)
                {
                    ((AttackMelee)enemy.GameObject.GetComponent(Tag.ATTACKMELEE)).MeleeAttack(enemy.GameObject, Tag.ENEMYMELEEATTACK, enemy.Velocity);
                     cooldownTimer = new TimeSpan(0, 0, 0, 0, 500);
                }

                if (((AttackMelee)enemy.GameObject.GetComponent(Tag.ATTACKMELEE)).canAttack == false)
                {
                    cooldownTimer -= GameWorld.Instance.ElapsedGameTime;

                    if (cooldownTimer <= TimeSpan.Zero)
                    {
                        ((AttackMelee)enemy.GameObject.GetComponent(Tag.ATTACKMELEE)).canAttack = true;
                    }
                }
            }

            enemy.GeneratePath();
            enemy.FollowPath(false);


        }

        public void RangedEnemyAttack()
        {
            float vectorX;
            float vectorY;

            enemy.Velocity = Vector2.Zero;
            enemy.Path.Clear();

            vectorX = (enemy.Target.Transform.Position.X) - (enemy.GameObject.Transform.Position.X);
            vectorY = (enemy.Target.Transform.Position.Y) - (enemy.GameObject.Transform.Position.Y);

            ((RangedAttack)enemy.GameObject.GetComponent(Tag.RANGEDATTACK)).RangedAttackMethod(enemy.GameObject, Tag.ENEMYPROJECTILE, new Vector2(vectorX, vectorY));
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
