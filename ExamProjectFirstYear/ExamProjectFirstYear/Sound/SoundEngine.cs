using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    class SoundEngine
    {
        private static SoundEngine instance;
        private Queue<SoundEffect> queue = new Queue<SoundEffect>();
        

        public static SoundEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundEngine();
                }

                return instance;
            }
        }

        public SoundEffect Footsteps { get => footsteps;}

        private Song ambience;
        private SoundEffect footsteps;

        public void LoadContent(ContentManager contentManager)
        {
            ambience = contentManager.Load<Song>("SoundTrack_mixdown2");
            footsteps = contentManager.Load<SoundEffect>("footsteps");
        }

        public void PlayAmbience()
        {
            MediaPlayer.Play(ambience);
          
            MediaPlayer.IsRepeating = true;
        }

        public void AddSoundEffect(SoundEffect soundEffect)
        {
            if (!queue.Contains(soundEffect))
            {
                queue.Enqueue(soundEffect);
            }

        }

        public void PlaySoundEffects()
        {
            //if (queue.Count < 0 || queue != null)
            //{
            //    SoundEffectInstance instance = queue.Dequeue().CreateInstance();

            //    if (instance.State != SoundState.Playing)
            //    {
            //        instance.Play();
            //    }

            //}

        }
    }
}
