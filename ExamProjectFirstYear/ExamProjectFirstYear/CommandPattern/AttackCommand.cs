using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
    /// <summary>
    /// Attack command for player.
    /// </summary>
    class AttackCommand : ICommand
    {
        #region Fields

        private int attackNumber;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for AttackCommand
        /// </summary>
        /// <param name="attackNumber"></param>
        public AttackCommand(int attackNumber)
        {
            this.attackNumber = attackNumber;
        }

        #endregion


        #region Methods

        public void Execute(Player player)
        {
            //switch (attackNumber)
            //{
            //    case 1:
            //        AttackMelee playerAttackMelee = (AttackMelee)player.GameObject.GetComponent(Tag.ATTACKMELEE);
            //        playerAttackMelee.MeleeAttack(player, player.Velocity);
            //        break;

            //    case 2:
            //        //RangedAttack rangedAttack = (RangedAttack)player.GameObject.GetComponent(Tag.RANGEDATTACK);
            //        //rangedAttack.RangedAttackMethod(player,player.Velocity);
            //        player.
            //        break;
            //}
            player.Attack(attackNumber);
        }

        public CommandTag GetCommandTag()
        {
            return CommandTag.KEYDOWN;
        }

        #endregion
    }
}
