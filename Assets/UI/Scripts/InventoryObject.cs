using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using UnityEngine.UI;

public enum InterfaceType
{
    Inventory,
    Equipment, 
    Chest
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string SavePath;
    public ItemDatabaseObject databaseObject;
    public InterfaceType type;
    public Inventory Container;
    public InventorySlot[] GetSlots
    {
        get
        {
            return Container.slots;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(item);
        if (!databaseObject.ItemObjects[item.Id].stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot FindSlotOfType(ItemType itemType)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            for (int j = 0; j < GetSlots[i].AllowedItems.Length; j++)
            {
                if (GetSlots[i].AllowedItems[j] == itemType)
                {
                    return GetSlots[i];
                }
            }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            Debug.Log(databaseObject.ItemObjects[item.Id]);
            if (GetSlots[i].item.Id <= -1 && GetSlots[i].CanPlaceInSlot(databaseObject.ItemObjects[item.Id]))
            {
                GetSlots[i].UpdateSlot(item, amount);
                return GetSlots[i];
            }
        }
        //Set up functionality for what happpens when inventory is full
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == item)
            {
                if (GetSlots[i].amount > 1)
                {
                    GetSlots[i].UpdateSlot(GetSlots[i].item, GetSlots[i].amount - 1);
                } else
                {
                    GetSlots[i].UpdateSlot(null, 0);
                }
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, SavePath))) {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot( newContainer.slots[i].item, newContainer.slots[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[15];

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterace parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                return parent.inventory.databaseObject.ItemObjects[item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public void UpdateSlot(Item itemObject, int objectAmount)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }
        item = itemObject;
        amount = objectAmount;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }
    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item itemObject, int objectAmount)
    {
        UpdateSlot(itemObject, objectAmount);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
        {
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }
        Debug.Log(_itemObject.type);
        return false;
    }
}
