using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Item List", menuName = "World Item List")]
public class WorldItemList : ScriptableObject
{
    public string SavePath;
    public ItemDatabaseObject ItemDatabase;
    public ItemList WorldItems;
    public GameObject groundObjectPrefab;

    public void Save()
    {

        string saveData = JsonUtility.ToJson(WorldItems, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, SavePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), WorldItems);
            file.Close();
        }

        for (int i = 0; i < WorldItems.groundItems.Count; i++)
        {
            GameObject newObject = Instantiate(groundObjectPrefab);
            newObject.transform.position = WorldItems.itemPositions[i];
            newObject.GetComponent<GroundItemHandler>().groundItem.item = ItemDatabase.ItemObjects[WorldItems.groundItems[i].Id];
            newObject.GetComponent<Billboard>().camera = Camera.main;
            newObject.GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.ItemObjects[WorldItems.groundItems[i].Id].uiDisplay;
        }
    }
}

[System.Serializable]
public class ItemList
{
    public List<Item> groundItems = new List<Item>();
    public List<Vector3> itemPositions = new List<Vector3>();
}
