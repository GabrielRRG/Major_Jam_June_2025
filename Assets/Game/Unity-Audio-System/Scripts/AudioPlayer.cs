using UnityEngine;

namespace RadiantTools.AudioSystem
{
    public class AudioPlayer : MonoBehaviour
    {
        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudioOnce(SoundTypes soundType)
        {
            //If audioSource is null then assign one
            if(audioSource == null) {
                audioSource = GetComponent<AudioSource>(); 
            }
            //Play the audio
            audioSource.PlayOneShot(GetAudioClip(soundType));
        }

        /// <summary>
        /// Sets the audioSource clip
        /// </summary>
        /// <param name="audioClip">the audio you want to set</param>
        public void SetAudioClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }
        /// <summary>
        /// Plays the audio clip already set in audioSource
        /// </summary>
        public void PlayAudio()
        {
            audioSource.Play();
        }

        public void SetAudioVolume(float volume)
        {
            audioSource.volume = volume;
        }
        public void SetAudioPitch(float pitch)
        {
            audioSource.pitch = pitch;
        }
        public void SetAudioLoop(bool loop)
        {
            audioSource.loop = loop;
        }
        public void SetAudioStart(bool playOnStart)
        {
            audioSource.playOnAwake = playOnStart;
        }

        /// <summary>
        /// Changes audio settings
        /// </summary>
        public void SetAudioSettings(float volume = 1f, float pitch = 1f, bool loop = false, bool playOnStart = false)
        {
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.loop = loop;
            audioSource.playOnAwake = playOnStart;
        }
        public AudioClip GetAudioClip(SoundTypes soundType)
        {
            foreach (SoundClips soundClip in AudioManager.Instance.audioList)
            {
                if (soundClip.soundType.Equals(soundType))
                {
                    return soundClip.audioClip;
                }
            }
            Debug.Log("ERROR -> No sound type found");
            return null;
        }
    }
}
