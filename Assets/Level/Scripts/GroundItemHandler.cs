using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundItemHandler : MonoBehaviour
{
    public GroundItem groundItem;
    private Player player;

    void Start()
    {
        groundItem.position = this.transform.position;
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        player.saveItemData.AddListener(RegisterItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterItem()
    {
        player.worldItemList.WorldItems.groundItems.Add(new Item(groundItem.item));
        player.worldItemList.WorldItems.itemPositions.Add(groundItem.position);
    }
}
