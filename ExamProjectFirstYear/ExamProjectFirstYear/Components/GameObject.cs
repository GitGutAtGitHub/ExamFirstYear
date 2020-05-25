using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    public class GameObject
    {
        #region FIELDS
        private Dictionary<Tag, Component> components = new Dictionary<Tag, Component>();
        //drawnComponents is used in the Draw method, so that only components that need to be drawn
        // such as SpriteRenderer call their Draw method.
        private Dictionary<Tag, Component> drawnComponents = new Dictionary<Tag, Component>();
        #endregion


        #region PROPERTIES
        public Transform Transform { get; private set; } = new Transform();
        public string SpriteName { get; set; }
        public Tag Tag { get; set; }
        public Dictionary<Tag, Component> Components { get => components; }
        #endregion


        #region METHODS
        public GameObject()
        {

        }

        /// <summary>
        /// Adds a component to the the GameObject.
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            Components.Add(component.ToEnum(), component);
            if (component.ToEnum() == Tag.SPRITERENDERER || component.ToEnum() == Tag.COLLIDER)
            {
                drawnComponents.Add(component.ToEnum(), component);
            }
            component.GameObject = this;
        }

        /// <summary>
        /// Returns a component based on their tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Component GetComponent(Tag tag)
        {
            return Components[tag];
        }

        /// <summary>
        /// Calls Awake for all the components in the GameObject.
        /// </summary>
        public void Awake()
        {
            foreach (Component component in Components.Values)
            {
                component.Awake();
            }
        }

        /// <summary>
        /// Calls Start for all the components in the GameObject
        /// </summary>
        public void Start()
        {
            foreach (Component component in Components.Values)
            {
                component.Start();
            }
        }



        /// <summary>
        /// Calls Update for all the components in the GameObject
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (Component component in drawnComponents.Values)
            {
                component.Update(gameTime);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)

        {
            foreach (Component component in drawnComponents.Values)
            {
                component.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Ensures that every Component in the GameObject is destroyed before the GameObject is deleted.
        /// </summary>
        public void Destroy()
        {
            foreach (Component component in Components.Values)
            {
                component.Destroy();
            }

            GameWorld.Instance.DeleteGameObject(this);
        }


        //public override Tag ToEnum()
        //{
        //	//return Tag = Tag.GAMEOBJECT;
        //}


        /// <summary>
        /// Returns how many nodes that object is occupying
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <returns></returns>
        public float GetObjectWidthInCellSize(SpriteRenderer spriteRenderer)
        {
            return spriteRenderer.Sprite.Width / NodeManager.Instance.CellSize;
        }

        public float GetObjectHeightInCellSize(SpriteRenderer spriteRenderer)
        {
            return spriteRenderer.Sprite.Height / NodeManager.Instance.CellSize;
        }


        public Vector2 GetCoordinate()
        {
            return Transform.Position / NodeManager.Instance.CellSize;
        }
        #endregion
    }
}
