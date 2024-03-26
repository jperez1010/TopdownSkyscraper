using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSavePoint : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("InkJSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private Player player;

    private void Awake()
    {
        playerInRange = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        visualCue.SetActive(false);
    }
    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, this, player);
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
            player = other.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            player = null;
        }
    }

    public void SaveGame(Player player)
    {
        player.playerLocation.location = player.transform.position;
        player.inventory.Save();
        player.equipment.Save();
        player.playerLocation.Save();
        player.worldItemList.WorldItems.groundItems.Clear();
        player.worldItemList.WorldItems.itemPositions.Clear();
        player.saveItemData.Invoke();
        StartCoroutine(player.SaveWorldItems());
    }
}
