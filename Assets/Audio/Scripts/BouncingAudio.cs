using UnityEngine;
using FMODUnity;

public class BouncingAudio : MonoBehaviour
{
    public float maxDistance = 10f; 
    public float minDistance = 1f; 
    private string cutOutTag = "CutOutTag"; 
    public float cutOutDistance = 5f;
    public float c = 1f;

    public GameObject player; 

    FMOD.Studio.EventInstance musicDemo; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        musicDemo = FMODUnity.RuntimeManager.CreateInstance("event:/Music/CyberStage"); 
        musicDemo.start(); 
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 emitterPosition = transform.position;
            Vector3 listenerPosition = player.transform.position;

            float distance = Vector3.Distance(emitterPosition, listenerPosition);

            RaycastHit[] hits = Physics.RaycastAll(emitterPosition, listenerPosition - emitterPosition, distance);

            float obstructionAmount = 0f;
            bool inaudible = false;

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue; 
                }

                if (hit.collider.CompareTag("Obstacle"))
                {
                   
                    obstructionAmount =  1-Mathf.Exp(-c*(distance / hit.distance - 1));
                }

                
                if (hit.collider.CompareTag(cutOutTag) && distance < cutOutDistance)
                {
                    inaudible = true;
                    break;
                }
            }

            
            obstructionAmount = Mathf.Clamp01(obstructionAmount);

            
            musicDemo.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(emitterPosition));

           
            musicDemo.setParameterByName("obstruction", obstructionAmount);

            
            float volume = 1f;

            if (inaudible)
            {
                volume = 0f; 
            }
            else
            {
                volume = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            }

            musicDemo.setVolume(volume);
        }
    }

    void OnDestroy()
    {
        musicDemo.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); 
        musicDemo.release(); 
    }
}