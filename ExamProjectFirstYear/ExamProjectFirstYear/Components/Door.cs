using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Door component class.
    /// Made public so we can access this class from GameWorld.
    /// </summary>
    public class Door : Component
    {
        #region Fields

        private bool isLocked;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for Door.
        /// </summary>
        public Door()
        {

        }

        #endregion


        #region Override methods

        public override void Awake()
        {
            GameObject.Tag = Tag.DOOR;
            GameObject.SpriteName = "UnWalkableNode";
        }

        public override void Start()
        {
            isLocked = true;
        }

        public override Tag ToEnum()
        {
            return Tag.DOOR;
        }

        #endregion


        #region Other methods

        /// <summary>
        /// Method for setting a door to unlocked after opening it.
        /// </summary>
        public void OpenDoor()
        {
            isLocked = false;
        }

        #endregion
    }
}