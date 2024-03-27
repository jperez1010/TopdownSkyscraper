using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDogSFX : MonoBehaviour
{
    public void PlayWalk()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundEffects/Enemy/DAWG_WALK", this.transform.position);
    }

    public void PlayBark()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SoundEffects/Enemy/DAWG", this.transform.position);
    }
}
