using ExamProjectFirstYear.MenuStatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern.MenuCommandPattern
{
    class PauseMenuCommand : IMenuCommand
    {
        public PauseMenuCommand()
        {

        }

        public void Execute()
        {
            if (MenuHandler.Instance.GameState == GameState.PlayingState)
            {
                MenuHandler.Instance.GameShouldBePaused = true;
            }

            else if (MenuHandler.Instance.GameState == GameState.PausedState)
            {
                MenuHandler.Instance.GameShouldBePaused = false;
            }

            //if (MenuHandler.Instance.GameState == GameState.PlayingState)
            //{
            //    MenuHandler.Instance.SwitchState(new PausedState());
            //}

            //else if (MenuHandler.Instance.GameState == GameState.PausedState)
            //{
            //    MenuHandler.Instance.SwitchState(new PlayingState());
            //}
        }
    }
}
