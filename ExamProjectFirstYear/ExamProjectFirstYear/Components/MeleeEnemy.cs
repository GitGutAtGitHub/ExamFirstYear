using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class MeleeEnemy : Component
    {
        public override Tag ToEnum()
        {
            return Tag.MEELEEENEMY;
        }


        public void DropMaterial(int materialID)
        {
            GameObject createdObject = new GameObject();
            Material material;

            createdObject.AddComponent(material = new Material(materialID));

            createdObject.Awake();
            createdObject.Start();

            GameWorld.Instance.GameObjects.Add(createdObject);
        }
    }
}
