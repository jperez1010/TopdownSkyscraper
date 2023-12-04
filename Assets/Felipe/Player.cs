using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public PlayerLocation playerLocation;

    [SerializeField]
    private DisplayInventory displayInventory;
    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerLocation.location = transform.position;
            inventory.Save();
            playerLocation.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            playerLocation.Load();
            gameObject.transform.position = playerLocation.location;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (displayInventory.gameObject.activeSelf)
            {
                Time.timeScale = 1f;
                displayInventory.gameObject.SetActive(false);
            } else
            {
                Time.timeScale = 0f;
                displayInventory.gameObject.SetActive(true);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.items = new InventorySlot[15];
    }
}
