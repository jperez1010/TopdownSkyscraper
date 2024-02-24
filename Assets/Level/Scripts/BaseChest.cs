using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChest : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Grid Inventory")]
    [SerializeField] private GameObject Grid;

    [Header("Player Inventory")]
    [SerializeField] private GameObject PlayerInventoryUI;

    private bool playerInRange;

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
                if (Grid.activeSelf)
                {
                    Grid.SetActive(false);
                    PlayerInventoryUI.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    Grid.SetActive(true);
                    PlayerInventoryUI.SetActive(true);
                    Time.timeScale = 0f;
                }
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    /*public void SaveGame(Player player)
    {
        player.playerLocation.location = transform.position;
        player.inventory.Save();
        player.equipment.Save();
        player.playerLocation.Save();
        player.worldItemList.WorldItems.groundItems.Clear();
        player.worldItemList.WorldItems.itemPositions.Clear();
        player.saveItemData.Invoke();
        StartCoroutine(player.SaveWorldItems());
    }*/
}
