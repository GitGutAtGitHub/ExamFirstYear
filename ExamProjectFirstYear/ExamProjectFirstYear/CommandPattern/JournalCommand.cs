using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern.MenuCommandPattern
{
    class JournalCommand : ICommand
    {
        private byte action;

        public JournalCommand(byte action)
        {
            this.action = action;
        }
        public void Execute(Player player)
        {
            if (action == 1)
            {
                GameWorld.Instance.Journal.HandleJournal();
            }
            if (action == 2)
            {
                GameWorld.Instance.Journal.ChangePage();
            }
        }
    }
}
