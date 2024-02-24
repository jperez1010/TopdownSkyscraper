using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : GeneralUserInterface
{
    private ItemGrid selectedItemGrid;

    public InventoryObject equipment;
    public UserInterace equipmentInterface;
    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    public InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField]
    List<ItemObject> items;
    [SerializeField]
    public ItemDatabaseObject databaseObject;
    [SerializeField]
    public GameObject itemPrefab;
    [SerializeField]
    Transform CanvasTransform;

    InventoryHighlight inventoryHighlight;

    public bool unequippingItem = false;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
        }
    }

    private void Update()
    {
        ItemIconDrag();

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                CreateRandomItem();
            }
        }*/

        /*if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }*/

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null || !selectedItemGrid.gameObject.activeInHierarchy)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            MouseClickOnGrid();
        }

        if (Input.GetMouseButtonDown(1))
        {
            EquipItem();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        
        if (selectedItemGrid == null)
        {
            return;
        }
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    public void InsertEquippedItem(InventoryItem itemToInsert)
    {
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid && !Input.GetKeyDown(KeyCode.R) || unequippingItem)
        {
            return;
        }
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            } else
            {
                inventoryHighlight.Show(false);
            }
        } else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(CanvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID].data, databaseObject.ItemObjects[items[selectedItemID].data.Id].uiDisplay);
    }

    public void AddItem(Item itemData, ItemGrid itemGrid, InventorySlot newSlot)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(CanvasTransform);
        rectTransform.SetAsLastSibling();

        inventoryItem.Set(itemData, databaseObject.ItemObjects[itemData.Id].uiDisplay);

        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        selectedItemGrid = itemGrid;
        InsertItem(itemToInsert);
        selectedItemGrid = null;
        itemGrid.slotsOnInterface.Add(itemToInsert, newSlot);
    }

    private void MouseClickOnGrid()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void EquipItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }

        if (selectedItem != null)
        {
            InventorySlot targetSlot = equipment.FindSlotOfType(equipment.databaseObject.ItemObjects[selectedItem.itemData.Id].type);
            if (targetSlot.item.Id >= 0)
            {
                UserInterace UI = (UserInterace)targetSlot.parent;
                UI.AddToGridInventory(this, selectedItemGrid, targetSlot);
            }
            inventory.SwapItems(selectedItemGrid.slotsOnInterface[selectedItem], targetSlot);
            Destroy(selectedItem.gameObject);
            //inventoryHighlight.Show(false);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;

        }

        Vector2Int tileGridPosition = selectedItemGrid.GetGridPosition(position);
        return tileGridPosition;
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool itemPlaced = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (itemPlaced)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
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
