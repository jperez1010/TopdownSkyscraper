using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterace : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public GameObject groundObjectPrefab;
    public InventoryObject inventory;

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        slotsOnInterface.UpdateSlotDisplay();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        gameObject.SetActive(false);
    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.item.Id >= 0)
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject.uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null; ;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    slotsOnInterface.UpdateSlotDisplay();
    //}

    public abstract void CreateSlots();

    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterace>();
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }

        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {

        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            GameObject newObject = Instantiate(groundObjectPrefab);
            GameObject player = GameObject.FindWithTag("Player");
            newObject.transform.position = player.transform.position + new Vector3(player.transform.forward.x * 2, -player.transform.position.y, player.transform.forward.z * 2);
            newObject.GetComponent<GroundItemHandler>().groundItem.item = slotsOnInterface[obj].ItemObject;
            newObject.GetComponent<Billboard>().camera = Camera.main;
            newObject.GetComponentInChildren<SpriteRenderer>().sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            if (slotsOnInterface[obj].amount > 1)
            {
                slotsOnInterface[obj].UpdateSlot(slotsOnInterface[obj].item, slotsOnInterface[obj].amount - 1);
            } else
            {
                slotsOnInterface[obj].RemoveItem();
            }
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnMouseClick(GameObject obj)
    {
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            InventoryController inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
            ItemGrid itemGrid = FindObjectOfType(typeof(ItemGrid)) as ItemGrid;
            AddToGridInventory(inventoryController, itemGrid, slotsOnInterface[obj]);
            inventoryController.SelectedItemGrid = null;
        }
    }

    public void AddToGridInventory(InventoryController inventoryController, ItemGrid itemGrid, InventorySlot obj)
    {
        inventoryController.unequippingItem = true;
        inventoryController.SelectedItemGrid = itemGrid;
        InventoryItem inventoryItem = Instantiate(inventoryController.itemPrefab).GetComponent<InventoryItem>();
        inventoryItem.itemData = obj.item;
        if (inventoryController.SelectedItemGrid.FindSpaceForObject(inventoryItem) == null)
        {
            Destroy(inventoryItem.gameObject);
            inventoryController.SelectedItemGrid = null;
            inventoryController.unequippingItem = false;
            return;
        }
        inventoryItem.Set(inventoryItem.itemData);
        inventoryController.InsertEquippedItem(inventoryItem);
        obj.UpdateSlot(new Item(), 0);
        inventoryController.unequippingItem = false;
    }
}

public static class MouseData
{
    public static UserInterace interfaceMouseIsOver;

    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _slotsOnInterface)
        {
            if (slot.Value.item.Id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null; ;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
