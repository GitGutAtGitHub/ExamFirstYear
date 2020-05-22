using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// Transform class used for moving objects in-game.
    /// </summary>
	public class Transform
	{
        #region PROPERTIES

        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        #endregion


        #region METHODS

        /// <summary>
        /// Used to set and update the position of a GameObject.
        /// </summary>
        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }

        #endregion
    }
}
