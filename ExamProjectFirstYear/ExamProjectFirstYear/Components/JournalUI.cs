using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class JournalUI : Component, IGameListener
    {
        #region Fields

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        private SpriteRenderer spriteRenderer;


        #endregion


        public override Tag ToEnum()
        {
            return Tag.JOURNAILUI;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.JOURNAILUI;
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
        }

        public override void Start()
        {
            GameObject.Transform.Translate(new Vector2(30, 30));
            GameObject.SpriteName = "ClosedJournal";
        }

        public override void Update(GameTime gameTime)
        {
            OpenJournal();
            CloseJournal();
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            
        }

        private void OpenJournal()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                GameObject.SpriteName = "OpenJournal";
            }
        }

        private void CloseJournal()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                GameObject.SpriteName = "ClosedJournal";
            }
        }
    }
}
