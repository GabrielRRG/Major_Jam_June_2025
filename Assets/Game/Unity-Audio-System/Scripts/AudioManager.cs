using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadiantTools.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        public SoundClips[] audioList;

        //Just your typical singleton pattern.
        public static AudioManager Instance { get; private set; }
        public static List<AudioPlayer> audioPlayerList = new List<AudioPlayer>(); 
        void Start()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            
            MakeAudioPlayer("SoundSFX");
            //Setting Music Player
            AudioPlayer musicPlayer = MakeAudioPlayer("Music");
            musicPlayer.SetAudioSettings(volume : 1, loop : true , playOnStart : true);
            musicPlayer.SetAudioClip(musicPlayer.GetAudioClip(SoundTypes.JungleMusic));
            musicPlayer.PlayAudio();
        }

        /// <summary>
        /// Make an Audio Player
        /// </summary>
        /// <param name="name">name for the audio player</param>
        /// <returns>The created audio player</returns>
        public AudioPlayer MakeAudioPlayer(string name)
        {
            //Making a new AudioObject
            GameObject audioObject = new GameObject(name);
            audioObject.transform.SetParent(transform);
            //Adding an AudioSource & AudioPlayer to the AudioObject
            audioObject.AddComponent<AudioSource>();
            AudioPlayer audioPlayer = audioObject.AddComponent<AudioPlayer>();
            audioPlayerList.Add(audioPlayer);
            return audioPlayer;
        }

        /// <summary>
        /// Deletes an Audio Player
        /// </summary>
        /// <param name="name">Name of the Audio Player to want to delete</param>
        public void DeleteAudioPlayer(string name)
        {
            foreach(Transform audioTransform in transform)
            {
                if(audioTransform.name == name)
                {
                    Destroy(audioTransform.gameObject);
                    return;
                }
            }
        }
        /// <summary>
        /// Gets a particular audio player
        /// </summary>
        /// <param name="name">name of the audio player you want</param>
        /// <returns>audio player if found otherwise returns null</returns>
        public AudioPlayer GetAudioPlayer(string name)
        {
            foreach (Transform audioTransform in transform)
            {
                if (audioTransform.name == name)
                {
                    return audioTransform.GetComponent<AudioPlayer>();
                }
            }
            //If no audioPlayer is found
            return null;
        }
    }
}
