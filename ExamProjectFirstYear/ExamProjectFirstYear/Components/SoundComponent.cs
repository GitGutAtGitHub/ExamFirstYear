using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class SoundComponent : Component
    {
        private List<SoundObject> soundObjects = new List<SoundObject>();

        private float distance2;
        private float pan;

        private float volume = 0;

        private float range = 1000;

        public float Volume { get => volume; set => volume = value; }

        public void UpdateDistance()
        {
            distance2 = (int)Math.Sqrt((Camera.Instance.Position.X - GameObject.Transform.Position.X) * (Camera.Instance.Position.X - GameObject.Transform.Position.X) +
                                  (Camera.Instance.Position.Y - GameObject.Transform.Position.Y) * (Camera.Instance.Position.Y - GameObject.Transform.Position.Y));

            pan =  ((GameObject.Transform.Position.X - Camera.Instance.Position.X) / range);
            volume = ( 1 - (distance2 / range));
        }

        public override void Start()
        {
            foreach (SoundObject SO in soundObjects)
            {
                SO.soundInstance.Volume = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateDistance();

            KeepAudioLooping();

            foreach (SoundObject SO in soundObjects)
            {
                AdjustVolumeAndPanning(SO.soundInstance);
            }
        }

        public void KeepAudioLooping()
        {
            foreach (SoundObject SO in soundObjects)
            {
                if (SO.repeat == true)
                {
                    if (SO.soundInstance.State != SoundState.Playing)
                    {
                        SO.soundInstance.Play();
                    }

                }
            }
        }


        public void StopPlayingSound(string soundName)
        {
            foreach (SoundObject SO in soundObjects)
            {
                if (SO.name == soundName)
                {
                    SO.soundInstance.Stop();
                }
            }
            //soundInstances.Add(tmp.CreateInstance());
        }

        public void StartPlayingSoundEffect(string soundName)
        {
            foreach (SoundObject SO in soundObjects)
            {
                if (SO.name == soundName)
                {
                   
                    SO.sound.Play();

                }
            }
            //soundInstances.Add(tmp.CreateInstance());
        }

        public void StartPlayingSoundInstance(string soundName)
        {
            foreach (SoundObject SO in soundObjects)
            {
                if (SO.name == soundName)
                {
                    if (SO.soundInstance.State != SoundState.Playing)
                    {
                        SO.soundInstance.Play();
                       
                    }

                }
            }
            //soundInstances.Add(tmp.CreateInstance());
        }

        public void AdjustVolumeAndPanning(SoundEffectInstance soundInstance)
        {

            foreach (SoundObject SO in soundObjects)
            {
                if (SO.soundInstance.State == SoundState.Playing)
                {
                    
                    if (volume > 0 && volume <= 1 )
                    {
                        soundInstance.Volume = Math.Abs(volume) / 2f;

                        if (pan > 1)
                        {
                            pan = 1;
                        }
                        else if (pan <-1)
                        {
                            pan = -1;
                        }
                        else
                        {
                            soundInstance.Pan = (pan);
                        }
                       
                    }
                    else
                    {
                        volume = 0;
                    }
                }
            }
          
        }

        public void ChangeRepeat(string soundName, bool newValue)
        {
            for (int i = 0; i < soundObjects.Count; i++)
            {
                if (soundObjects[i].name == soundName)
                {
                    soundObjects[i].ChangeRepeat(newValue);
                }
            }
        }
       

        public void AddSound(string soundName, bool repeat)
        {
            soundObjects.Add(new SoundObject(soundName, repeat));
        }

        public override void Destroy()
        {
            foreach (SoundObject SO in soundObjects)
            {
                SO.repeat = false;
                SO.soundInstance.Stop();

            }
        }


        public override Tag ToEnum()
        {
            return Tag.SOUNDCOMPONENT;
        }
    }

    public class SoundObject
    {
        public SoundEffect sound;
        public SoundEffectInstance soundInstance;
        public string name;
        public bool repeat;

        public SoundObject(string soundName, bool repeat)
        {
            this.name = soundName;
            sound = GameWorld.Instance.Content.Load<SoundEffect>(soundName);
            soundInstance = sound.CreateInstance();
            this.repeat = repeat;

        }

        public void ChangeRepeat(bool newValue)
        {
            this.repeat = newValue;
        }
    }
}
