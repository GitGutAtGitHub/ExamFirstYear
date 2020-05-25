using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class Platform : Component
    {
        public override Tag ToEnum()
        {
            return Tag.PLATFORM;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.PLATFORM;
        }

        public override void Start()
        {

            GameObject.SpriteName = "Platform";
        }
    }
}
