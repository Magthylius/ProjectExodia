using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    [System.Serializable]
    public class SoundFile
    {
        public AudioClip clip;
        public string audioName;
        [Range(0.0f, 1.0f)] public float volume;
    }

    [System.Serializable]
    public class SoundPack
    {
        public string audioPackName;
        public List<SoundFile> audioList = new List<SoundFile>();
    }
}

