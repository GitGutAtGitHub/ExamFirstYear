using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Melee Enemy component class.
    /// </summary>
    public class MeleeEnemy : Component
    {
        #region Override methods

        public override Tag ToEnum()
        {
            return Tag.MEELEEENEMY;
        }

        #endregion


        #region Other methods

        public void DropMaterial(int materialID)
        {
            GameObject createdObject = new GameObject();
            Material material;

            createdObject.AddComponent(material = new Material(materialID));

            createdObject.Awake();
            createdObject.Start();

            GameWorld.Instance.GameObjects.Add(createdObject);
        }

        #endregion
    }
}
