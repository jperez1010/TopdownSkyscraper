using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWalkSFX : MonoBehaviour
{
    private FMOD.Studio.EventInstance walkingHuman;
    // Start is called before the first frame update
    void Start()
    {
        walkingHuman = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/Player/Walk/Walking");
        
    }

    public void PlayWalk()
    {
        Debug.Log("Pressed E on the shed");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundEffects/Player/Walk/Walking", this.transform.position);
    }


}
