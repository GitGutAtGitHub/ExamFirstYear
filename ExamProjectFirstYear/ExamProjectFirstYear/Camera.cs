using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class Camera
	{
        private static Camera instance;

        private Vector2 position;



        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }

                return instance;
            }
        }


        private Matrix transformCamera;

        //Makes sure transform can be called from other classes.
        //In this case, it's used in GameWorld.
        public Matrix TransformCamera
        {
            get { return transformCamera; }
        }

        public Vector2 Position { get => position; }

        /// <summary>
        /// Method to ensure the camera always follows a gameobject. In this case, it will follow the player.
        /// This method runs in GameWorld.
        /// </summary>
        /// <param name="player"></param>
        public void FollowPlayer(GameObject player)
        {

            position = player.Transform.Position;

            SpriteRenderer tmpRenderer = (SpriteRenderer)player.GetComponent(Tag.SPRITERENDERER);

            var playerPosition = Matrix.CreateTranslation(-player.Transform.Position.X - ((tmpRenderer.Sprite.Width / NodeManager.Instance.CellSize) / 2),
                                              -player.Transform.Position.Y - ((tmpRenderer.Sprite.Height / NodeManager.Instance.CellSize) / 2), 0);

            var middleOfScreen = Matrix.CreateTranslation(1920 / 2, 1080 / 2, 0);

            transformCamera = playerPosition * middleOfScreen;
        }
    }
}
