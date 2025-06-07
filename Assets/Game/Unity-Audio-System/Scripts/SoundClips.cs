using System;
using UnityEngine;

namespace RadiantTools.AudioSystem
{
    [Serializable]
    public class SoundClips
    {
        public SoundTypes soundType;
        public AudioClip audioClip;
    }
    public enum SoundTypes
    {
        MenuMusic,
        ArtifactMusic,
        JungleMusic,
        Gunshot1,
        Gunshot2,
        Gunshot3,
        Reload,
        Walking,
    }
}
