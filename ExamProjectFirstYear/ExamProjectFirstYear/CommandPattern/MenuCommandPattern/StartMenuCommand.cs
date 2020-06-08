using ExamProjectFirstYear.MenuStatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern.MenuCommandPattern
{
    class StartMenuCommand : IMenuCommand
    {
        private byte chosenOption;
        private bool canUseStartMenu = true;

        public StartMenuCommand(byte chosenOption)
        {
            this.chosenOption = chosenOption;
        }

        public void Execute()
        {
            if (canUseStartMenu == true && chosenOption == 1)
            {
                MenuHandler.Instance.StartGameAtStartMenu = true;
                canUseStartMenu = false;
            }

            if (chosenOption == 2)
            {
                MenuHandler.Instance.ExitGameAtMenu = true;
            }
        }
    }
}
