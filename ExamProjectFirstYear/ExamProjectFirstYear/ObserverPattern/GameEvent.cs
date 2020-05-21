using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class GameEvent
	{
		#region FIELDS
		// List of all the IGameListener Objects that listen for the GameEvent. 
		private List<IGameListener> listeners = new List<IGameListener>();

		#endregion

		#region PROPERTIES
		/// <summary>
		/// The title or "name" of the GameEvent
		/// </summary>
		public string Title { get; private set; }

		#endregion

		#region METHODS
		public GameEvent(string title)
		{
			Title = title;
		}

        /// <summary>
        /// Attaches/adds an IGameListener to the list of listeners for the GameEvent
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IGameListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Detaches/removes a listener from the list of listeners for the GameEvent
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IGameListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Method that notifies every listener on the list that the GameEvent they are attached to has occurred. 
        /// </summary>
        /// <param name="other"></param>
        public void Notify(Component other)
        {
            foreach (IGameListener listener in listeners)
            {
                listener.Notify(this, other);
            }
        }
        #endregion
    }
}
