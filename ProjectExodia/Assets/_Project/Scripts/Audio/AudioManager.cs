using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class AudioManager : ManagerBase
    {
        public AudioSource sfxSource;
        public AudioSource bgmSource;
        
        [SerializeField] private float bgmVolume;
        [SerializeField] private float sfxVolume;

        [SerializeField] private AudioData bgmData;

        public override void Initialize(GameContext gameContext)
        {
            PlaySlapBGM();
        }

        public void SetVolume()
        {
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            sfxSource.volume = sfxVolume;
        }

        public void SetBGMVolume()
        {
            PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
            bgmSource.volume = bgmVolume;
        }
        
        SoundFile GetSound(AudioData audioType, string audioName)
        {
            List<SoundFile> soundFiles = new List<SoundFile>(audioType.audioList);

            foreach (var file in soundFiles)
            {
                if (file.audioName == audioName)
                {
                    return file;
                }
            }

            return null;
        }
        
        SoundFile GetRandomSound(AudioData audioType, string audioName)
        {
            List<SoundPack> temp = new List<SoundPack>(audioType.soundPacks);

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].audioPackName == audioName)
                {
                    return temp[i].audioList[Random.Range(0, temp[i].audioList.Count)];
                }
            }

            return null;
        }

        public void PlaySfx(AudioData audioData, string audioName)
        {
            SoundFile sound = GetSound(audioData, audioName);
            if (sound != null)
            {
                sfxSource.volume = sound.volume;
                sfxSource.PlayOneShot(sound.clip);
            }
        }

        public void PlayRandomSfx(AudioData audioData, string audioName)
        {
            SoundFile sound = GetRandomSound(audioData, audioName);
            if (sound != null)
            {
                sfxSource.volume = sound.volume;
                sfxSource.PlayOneShot(sound.clip);
            }
        }

        public void PlayLoopingSfx(AudioData audioData, string audioName)
        {
            SoundFile sound = GetSound(audioData, audioName);
            if (sound != null)
            {
                sfxSource.volume = sound.volume;
                sfxSource.clip = sound.clip;
                sfxSource.loop = true;

                sfxSource.Play();
            }
        }

        public void PlayBGM(AudioData audioData, string audioName)
        {
            SoundFile sound = GetSound(audioData, audioName);
            if (sound != null)
            {
                bgmSource.volume = sound.volume;
                bgmSource.clip = sound.clip;
                bgmSource.loop = true;

                bgmSource.Play();
            }
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }

        public void StopAllSound()
        {
            sfxSource.Stop();
            bgmSource.Stop();
        }
        
        //! Dirty Implementation
        public void PlaySlapBGM()
        {
            PlayBGM(bgmData, "BGM");
        }
    }
    
    
    
}
