using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class GameObject : Component
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
		public Tag Tag { get; private set; }

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
			components.Add(component.ToEnum(), component);
			if (component.ToEnum() == Tag.SPRITERENDERER || component.ToEnum() == Tag.COLLIDER)
			{
				drawnComponents.Add(component.ToEnum(), component);
			}
			component.GO = this;
		}

		/// <summary>
		/// Returns a component based on their tag.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public Component GetComponent(Tag tag)
		{
			return components[tag];
		}

		/// <summary>
		/// Calls Awake for all the components in the GameObject.
		/// </summary>
		public override void Awake()
		{
			foreach (Component component in components.Values)
			{
				component.Awake();
			}
		}

		/// <summary>
		/// Calls Start for all the components in the GameObject
		/// </summary>
		public override void Start()
		{
			foreach (Component component in components.Values)
			{
				component.Start();
			}
		}

		/// <summary>
		/// Calls Update for all the components in the GameObject
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
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
		public override void Draw(SpriteBatch spriteBatch)
		{
			foreach (Component component in drawnComponents.Values)
			{
				component.Draw(spriteBatch);
			}
		}

		/// <summary>
		/// Ensures that every Component in the GameObject is destroyed before the GameObject is deleted. 
		/// </summary>
		public override void Destroy()
		{
			foreach (Component component in components.Values)
			{
				component.Destroy();
			}

			GameWorld.Instance.DeleteGameObject(this);
		}

		public override Tag ToEnum()
		{
			return Tag = Tag.GAMEOBJECT;
		}

		#endregion
	}
}