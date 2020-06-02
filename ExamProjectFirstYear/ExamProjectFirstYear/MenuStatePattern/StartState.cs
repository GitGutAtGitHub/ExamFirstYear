using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.MenuStatePattern
{
    class StartState : IState
    {
        private MenuHandler menuHandler;

        public void Enter(IEntity entity)
        {
            menuHandler = (MenuHandler)menuHandler.GameObject.GetComponent(Tag.MENUHANDLER);
        }

        public void Execute()
        {
            menuHandler.st = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
        }

        public void Exit()
        {
            
        }
    }
}
