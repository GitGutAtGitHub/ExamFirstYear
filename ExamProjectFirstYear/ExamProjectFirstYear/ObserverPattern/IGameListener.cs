using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public interface IGameListener
	{
        /// <summary>
        /// Notifies that an event has occured and which component is related to that event. 
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        void Notify(GameEvent gameEvent, Component component);
    }
}
