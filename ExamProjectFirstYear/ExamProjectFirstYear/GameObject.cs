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

		#endregion

		#region PROPERTIES
		public Transform Transform { get; private set; }

		#endregion
	}
}