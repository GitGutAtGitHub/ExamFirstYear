using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class Collider : Component
	{
		#region FIELDS
		//Used to notify objects when they are colliding.
		private GameEvent collidingEvent = new GameEvent("Colliding");
		//The following to fields are used to notify objects that has collided when they are no longer colliding with eachother.
		private GameEvent noLongerCollidingEvent = new GameEvent("NoLongerColliding");
		private Collider currentCollisionCollider;

		private Vector2 size;
		private Vector2 origin;
		private Texture2D collisionTexture;

		#endregion

		#region PROPERTIES
		public bool CheckCollisionEvents { get; set; }

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle
				(
					(int)(GameObject.Transform.Position.X - origin.X),

					(int)(GameObject.Transform.Position.Y - origin.Y),

					(int)size.X,
					(int)size.Y
				);
			}
		}

		#endregion

		#region METHODS

		/// <summary>
		/// Constructor for Colliders that don't need an IGameListener attached
		/// </summary>
		/// <param name="spriteRenderer"></param>
		public Collider(SpriteRenderer spriteRenderer)
		{
			collisionTexture = GameWorld.Instance.Content.Load<Texture2D>("");
			this.origin = spriteRenderer.Origin;
			this.size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
		}

		/// <summary>
		/// Constructor for Colliders that do need an IGameListener attached
		/// </summary>
		/// <param name="spriteRenderer"></param>
		/// <param name="listener"></param>
		public Collider(SpriteRenderer spriteRenderer, IGameListener listener)
		{
			collidingEvent.Attach(listener);
			noLongerCollidingEvent.Attach(listener);
			collisionTexture = GameWorld.Instance.Content.Load<Texture2D>("CollisionBox");
			origin = spriteRenderer.Origin;
			size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
		}

		/// <summary>
		/// Method that notifies if an object is colliding with a different object.
		/// </summary>
		/// <param name="other"></param>
		public void OnColliding(Collider other)
		{
			// Following notifies an object when this collider intersects with the other objects collider.
			if (CheckCollisionEvents)
			{
				if (other != this)
				{
					if (CollisionBox.Intersects(other.CollisionBox))
					{
						// currentCollider is set to other so that we can ask it later if we are currently colliding with this specific collider
						currentCollisionCollider = other;
						collidingEvent.Notify(other);
					}
				}
			}
		}

		/// <summary>
		/// Method that notifies when an objects is no longer colliding with an object that it used to collide with.
		/// </summary>
		/// <param name="other"></param>
		public void OnNoLongerColliding(Collider other)
		{
			// If the currentCollider is not null but we no longer intersect with it, then it is reset as null and 
			// we notify the listener that the event onNoLongerCollidingEvent titled "NoLongerColliding" has occured.
			// This is to tell the "other" that hey you used to collide with this object but you no longer do
			// so that other can run any necessary code in this event. 
			if (currentCollisionCollider != null)
			{
				if (!CollisionBox.Intersects(other.CollisionBox) && other == currentCollisionCollider)
				{
					currentCollisionCollider = null;
					noLongerCollidingEvent.Notify(other);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//Draws the collision boxes around objects. Re-insert for debugging.
			spriteBatch.Draw(collisionTexture, CollisionBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
		}

		public override Tag ToEnum()
		{
			return Tag.COLLIDER;
		}

		#endregion
	}
}