using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;
    RectTransform rectTransform;

    [SerializeField]
    List<ItemData> items;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    Transform CanvasTransform;

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseClickOnGrid();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(CanvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void MouseClickOnGrid()
    {
        Vector2Int tileGridPosition = selectedItemGrid.GetGridPosition(Input.mousePosition);
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItem = null;
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
