using UnityEngine;
using System;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource soundMusic;
    [SerializeField] private SoundType[] sounds;
    [SerializeField] bool isMute = false;
    [SerializeField] float volume;


    private void Start()
    {
        SetVolume(0.5f);
        //PlayMusic(Sounds.Music);
    }

    //we can access sound controll in unity
    public void SetVolume(float Volume)
    {
        volume = Volume;
        soundEffect.volume = volume;
        soundMusic.volume = volume;
    }

    //we can mute sound in unity inspector pannel
    public void Mute(bool status)
    {
        isMute = status;
    }

    public void PlayMusic(Sounds sound)
    {
        if (isMute)
            return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("clip not found for the soundType: " + sound);
        }
    }

    public void Play(Sounds sound)
    {
        if (isMute)
            return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("clip not found for the soundType: " + sound);
        }
    }

    //sound getter function
    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(sounds, i => i.soundType == sound);
        if (item != null)
            return item.audioClip;
        return null;
    }
}
[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip audioClip;
}
public enum Sounds
{
    ButtonClick,
    Music,
    HookThrow,
    CarCrush
}