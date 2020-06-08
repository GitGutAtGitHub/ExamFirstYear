using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
    class InventoryCommand : ICommand
    {
        public void Execute(Player player)
        {
            GameWorld.Instance.Inventory.HandleInventory();
        }

        public CommandTag GetCommandTag()
        {
            throw new NotImplementedException();
        }
    }
}
