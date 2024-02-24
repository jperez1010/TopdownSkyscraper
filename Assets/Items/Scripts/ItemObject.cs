using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CONSUMABLE,
    EQUIPMENT,
    DEFAULT,
    LEFT_LEG,
    RIGHT_LEG,
    LEFT_ARM,
    RIGHT_ARM,
    TORSO
}

public enum Attributes
{
    Strength,
    Stealth,
    Medicine,
    Gadget,
    Scavenger
}

[System.Serializable]
public abstract class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public GameObject characterDisplay;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item data = new Item();

    public List<string> boneNames = new List<string>();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

    private void OnValidate()
    {
        boneNames.Clear();
        if (characterDisplay == null)
        {
            return;
        }
        if (!characterDisplay.GetComponent<SkinnedMeshRenderer>())
        {
            return;
        }

        var renderer = characterDisplay.GetComponent<SkinnedMeshRenderer>();
        var bones = renderer.bones;

        foreach ( var t in bones )
        {
            boneNames.Add(t.name);
        }
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public int KeyId;
    public ItemBuff[] buffs;

    public int width = 1;
    public int height = 1;
    public bool Lshape;

    [System.NonSerialized]
    public Sprite itemIcon;

    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff: IModifier
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int minBuffQuant, int maxBuffQuant)
    {
        min = minBuffQuant;
        max = maxBuffQuant;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
