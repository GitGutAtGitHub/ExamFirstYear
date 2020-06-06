using ExamProjectFirstYear.StatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
    class PausedState : IState
    {
        public void Enter(IEntity entity)
        {
            
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }

        public Tag ToTag()
        {
            return Tag.ENEMYIDLESTATE;
        }
    }
}
