using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class MusicManager : Singleton<MusicManager>
{
    public FMOD.Studio.EventInstance musicEvent;
    public EventReference music;

    public void StartMusic()
    {
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(music);
        musicEvent.start();
    }
}
