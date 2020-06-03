using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class AttackMelee : Component, IAttack
    {
        public bool canAttack { get; set; } = true;
        public bool canShoot { get; set; } = true;


        public void ReleaseAttack(int attackNumber)
        {
            if (attackNumber == 1)
            {
                canAttack = true;
            }
            if (attackNumber == 2)
            {
                canShoot = true;
            }
        }

        /// <summary>
        /// Players method for attacking.
        /// </summary>
        /// <param name="attackNumber"></param>
        public void Attack(int attackNumber)
        {
            switch (attackNumber)
            {
                case 1:
                    MeleeAttack();
                    break;

                    //case 2:
                    //    RangedAttack();
                    //    break;
            }
        }

        /// <summary>
        /// Melee attack for Player.
        /// </summary>
        private void MeleeAttack()
        {
            //if (canAttack)
            //{
            //    GameObject tmpMeleeObject = PlayerMeleeAttackPool.Instance.GetObject();

            //    SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
            //    Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);

            //    tmpMeleeObject.Transform.Position = GameObject.Transform.Position;

            //    GameWorld.Instance.GameObjects.Add(tmpMeleeObject);
            //    GameWorld.Instance.Colliders.Add(tmpMeleeCollider);
            //    canAttack = false;
            //}
        }

        public override Tag ToEnum()
        {
            return Tag.ATTACKMELEE;
        }
    }
}
