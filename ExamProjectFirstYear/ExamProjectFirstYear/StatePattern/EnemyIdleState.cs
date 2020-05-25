using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.StatePattern
{
    class EnemyIdleState : IState
    {

        private Enemy enemy;
        public void Enter(IEntity enemy)
        {
            this.enemy = enemy as Enemy;
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
