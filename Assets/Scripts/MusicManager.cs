using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class MusicManager : Singleton<MusicManager>
{
    public FMOD.Studio.EventInstance musicEvent;
    [FMODUnity.EventRef] public string music;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(music);
        musicEvent.start();
        musicEvent.setParameterByNameWithLabel("CurrentScreen", "Combat");
    }

    // Update is called once per frame

}
