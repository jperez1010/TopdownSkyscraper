using UnityEngine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    //EVENTS:
    private List<EventInstance> eventInstances;
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;
    private EventInstance lightBuzzInstance;
    private EventInstance startButtonInstance;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            eventInstances = new List<EventInstance>();
        }
        else
        {
            Debug.LogError("Found more than one audio manager Womp Womp");
            return;
        }
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }
    public void PlayOneShot(EventReference sound, GameObject targetObject)
    {
        EventInstance eventInstance = CreateInstance(sound);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(targetObject.transform));
        eventInstance.start();
    }
    public void ClearOneShot()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
    }
    public EventInstance CreateInstance (EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances) 
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
    private void OnDestroy()
    {
            CleanUp();
    }

    public void Start()
    {
        InitializeMusic(FMODEvents.instance.BGM);
    }
    private void InitializeAmbiance(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        musicEventInstance.start(); 
    }

    private void InitializeLightBuzz(EventReference lightBuzzEventReference, GameObject gameObject)
    {
        lightBuzzInstance = CreateInstance(lightBuzzEventReference);
        lightBuzzInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        lightBuzzInstance.start();
    }
    private void InitializeStartButton(EventReference startButtonReference)
    {
        startButtonInstance = CreateInstance(startButtonReference);
        startButtonInstance.start();
    }

}
