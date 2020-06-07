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
	/// Made public so we can access this class from other classes.
	/// </summary>
	public class Door : Component, IGameListener
	{
		#region Fields

		private bool isLocked = true;
		private Player player = GameWorld.Instance.player;

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
			Console.WriteLine("It worked hurray");
		}

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLAYER)
            {
				player.PlayerCollidingWithDoor = true;
			}
			
			if (gameEvent.Title == "NoLongerColliding" && component.GameObject.Tag == Tag.PLAYER)
            {
				player.PlayerCollidingWithDoor = false;
			}
		}

        #endregion
    }
}
