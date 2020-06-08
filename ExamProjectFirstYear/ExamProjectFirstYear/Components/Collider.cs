using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// Collider component class. Used for collision.
    /// </summary>
    public class Collider : Component
    {
        #region FIELDS

        ////Used to notify objects when they are colliding.
        private GameEvent collidingEvent = new GameEvent("Colliding");
        ////The following to fields are used to notify objects that has collided when they are no longer colliding with eachother.
        private GameEvent noLongerCollidingEvent = new GameEvent("NoLongerColliding");

        private List<Collider> listOfCurrentColliders = new List<Collider>();

        private Vector2 size;
        private Vector2 origin;

        private Texture2D collisionTexture;

        #endregion


        #region PROPERTIES

        public bool CheckCollisionEvents { get; set; } = false;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                (
                    (int)(GameObject.Transform.Position.X - origin.X),

                    (int)(GameObject.Transform.Position.Y - origin.Y),

                    (int)(size.X * GameWorld.Instance.Scale),
                    (int)(size.Y * GameWorld.Instance.Scale)
                );
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for Colliders that don't need an IGameListener attached
        /// </summary>
        /// <param name="spriteRenderer"></param>
        public Collider(SpriteRenderer spriteRenderer)
        {
            collisionTexture = GameWorld.Instance.Content.Load<Texture2D>("CollisionBox");
            origin = spriteRenderer.Origin;
            size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
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

        #endregion


        #region Override methods

        //public override void Awake()
        //{
        //	collisionTexture = GameWorld.Instance.Content.Load<Texture2D>("CollisionBox");
        //}

        //public override void Start()
        //{
        //	SpriteRenderer spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
        //	origin = spriteRenderer.Origin;
        //	size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
        //}

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


        #region Other methods

        public void AttachListener(IGameListener listener)
        {
            collidingEvent.Attach(listener);
            noLongerCollidingEvent.Attach(listener);
        }

        /// <summary>
        /// Method that notifies if an object is colliding with a different object.
        /// </summary>
        /// <param name="other"></param>
        public void OnColliding(Collider other)
        {
            // Following notifies an object when this collider intersects with the other objects collider.
            if (CheckCollisionEvents && other != this && CollisionBox.Intersects(other.CollisionBox))
            {
                //The other object is being notified of the collision.
                collidingEvent.Notify(other);

                if (!listOfCurrentColliders.Contains(other))
                {
                    // Adds the object to a list of things that are colliding.
                    listOfCurrentColliders.Add(other);
                }
            }
        }

        /// <summary>
        /// Method that notifies when an objects is no longer colliding with an object that it used to collide with.
        /// </summary>
        /// <param name="other"></param>
        public void OnNoLongerColliding(Collider other)
        {
            // If the list contains the collider, the object that is no longer being collided with,
            // the other object will be notified that there is no collision anymore
            // and the object is removed from the list of current colliders.
            if (listOfCurrentColliders.Contains(other))
            {
                if (!CollisionBox.Intersects(other.CollisionBox))
                {
                    noLongerCollidingEvent.Notify(other);
                    listOfCurrentColliders.Remove(other);
                }
            }
        }

        public override void Destroy()
        {
            GameWorld.Instance.Colliders.Remove(this);
        }

        #endregion
    }
}
