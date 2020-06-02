using ExamProjectFirstYear.StatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
    class LoadingState : IState
    {
        private MenuHandler menuHandler;

        public void Enter(IEntity entity)
        {
            menuHandler = (MenuHandler)menuHandler.GameObject.GetComponent(Tag.MENUHANDLER);
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
