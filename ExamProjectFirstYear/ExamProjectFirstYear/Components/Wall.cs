using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class Wall : Component
    {
        public override Tag ToEnum()
        {
            return Tag.WALL;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.PLATFORM;
        }

        public override void Start()
        {
            GameObject.SpriteName = "WallSprite";
        }
    }
}
