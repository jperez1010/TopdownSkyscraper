using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("InkJSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    public bool playerHasCorrectKey;
    [SerializeField] private int keyId;

    [SerializeField] private Animator myDoor;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                return;
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            for (int i = 0; i < other.GetComponent<Player>().inventory.GetSlots.Length; i++)
            {
                if (other.GetComponent<Player>().inventory.GetSlots[i].item.Id == 5 && other.GetComponent<Player>().inventory.GetSlots[i].item.KeyId == keyId)
                {
                    playerHasCorrectKey = true;
                    return;
                }
            }
            playerHasCorrectKey = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void OpenDoor()
    {
        myDoor.Play("doorOpen", 0, 0.0f);
        gameObject.SetActive(false);
    }
}
