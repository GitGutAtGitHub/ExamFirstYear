using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public abstract class Component
	{
        #region PROPERTIES
        /// <summary>
        /// Property used to get and set a gameObject outside of the Component class.
        /// </summary>
        public GameObject GameObject { get; set; }

		#endregion

		/// <summary>
		/// Awake is called once per component. Together with Start they in a sense replaces the LoadContent method.
		/// </summary>
		public virtual void Awake() { }

        /// <summary>
        /// Start is Called after Awake, once per component.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Update method. Updates the Component in GameTime.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draw for the Component. Is mainly used by the SpriteRenderer Component. 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Destroy for the component. Removes the Component from the Game. 
        /// </summary>
        public virtual void Destroy() { }

        /// <summary>
        /// Returns the components Tag. Is used to tell Components apart. 
        /// Mainly used by the GetComponent method defined by the GameObject class.
        /// </summary>
        /// <returns></returns>
        public abstract Tag ToEnum();
    }
}
