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


        public void ReleaseMeleeMeleeAttack()
        {
            canAttack = true;
        }

        /// <summary>
        /// Melee attack for Player.
        /// </summary>
        public void PlayerMeleeAttack(Player player)
        {
            //if (canAttack)
            //{
            //    GameObject tmpMeleeObject = PlayerMeleeAttackPool.Instance.GetObject();

            //    SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
            //    Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);

            //    tmpMeleeObject.Transform.Position = player.GameObject.Transform.Position;

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
