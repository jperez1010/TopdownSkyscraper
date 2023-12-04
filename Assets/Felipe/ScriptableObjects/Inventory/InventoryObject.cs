using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string SavePath;
    public ItemDatabaseObject databaseObject;
    public Inventory Container;

    public void AddItem(Item itemObject, int amount)
    {
        if (itemObject.buffs.Length > 0)
        {
            SetEmptySlot(itemObject, amount);
            return;
        }
        for (int i = 0; i < Container.items.Length; i++)
        {
            if (Container.items[i].ID == itemObject.Id)
            {
                Container.items[i].AddAmount(amount);
                return;
            }
        }
        SetEmptySlot(itemObject, amount);
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.items.Length; i++)
        {
            if (Container.items[i].ID <= -1)
            {
                Container.items[i].UpdateSlot(item.Id, item, amount);
                return Container.items[i];
            }
        }
        //Set up functionality for what happpens when inventory is full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.items.Length; i++)
        {
            if (Container.items[i].item == item)
            {
                Container.items[i].UpdateSlot(-1, null, 0);
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
            for (int i = 0; i < Container.items.Length; i++)
            {
                Container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item, newContainer.items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[15];
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public void UpdateSlot(int id, Item itemObject, int objectAmount)
    {
        ID = id;
        item = itemObject;
        amount = objectAmount;
    }
    public InventorySlot(int id, Item itemObject, int objectAmount)
    {
        ID = id;
        item = itemObject;
        amount = objectAmount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
