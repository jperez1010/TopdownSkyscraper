using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory playerInventory;
    public GameObject inventoryUI;
    public GameObject equipmentUI;
    private void Awake()
    {
        //inventoryUI.SetActive(true);
        //equipmentUI.SetActive(true);
        playerInventory = this;
        DontDestroyOnLoad(gameObject);
    }
}
