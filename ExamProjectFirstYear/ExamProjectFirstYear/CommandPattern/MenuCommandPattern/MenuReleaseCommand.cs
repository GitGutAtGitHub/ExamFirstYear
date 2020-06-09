using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern.MenuCommandPattern
{
    class MenuReleaseCommand : IMenuCommand
    {
        public MenuReleaseCommand()
        {

        }

        public void Execute()
        {
			MenuHandler.Instance.PauseButtonReleased = true;
        }
    }
}
