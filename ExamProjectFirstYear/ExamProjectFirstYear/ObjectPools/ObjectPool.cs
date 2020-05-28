using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	abstract class ObjectPool
	{
		#region FIELDS
		protected List<GameObject> activeGameObjects = new List<GameObject>();
		protected Stack<GameObject> inactiveGameObjects = new Stack<GameObject>();

		#endregion

		#region METHODS
		public GameObject GetObject()
		{
			GameObject gameObject;

			if (inactiveGameObjects.Count > 0)
			{
				gameObject = inactiveGameObjects.Pop();
			}

			else
			{
				gameObject = Create();
			}

			activeGameObjects.Add(gameObject);

			return gameObject;		
		}

		/// <summary>
		/// Removes an object from the list of GameObjects and the list of activeGameObjects, but pushes the GameObject to the
		/// stack of inactiveGameObjects. 
		/// </summary>
		/// <param name="gameObject"></param>
		public void ReleaseObject(GameObject gameObject)
		{
			GameWorld.Instance.DeleteGameObject(gameObject);
			activeGameObjects.Remove(gameObject);
			inactiveGameObjects.Push(gameObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected abstract GameObject Create();

		protected abstract void CleanUp(GameObject gameObject);
		#endregion
	}
}
