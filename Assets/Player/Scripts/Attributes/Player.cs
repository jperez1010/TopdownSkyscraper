using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryController gridController;
    public ItemGrid itemGrid;
    public WorldItemList worldItemList;
    public Attribute[] attributes;
    public PlayerLocation playerLocation;
    [SerializeField]
    private GameObject InventoryUI;
    [SerializeField]
    private GameObject EquipmentUI;
    public UnityEvent saveItemData;

    private BoneCombiner boneCombiner;

    private Transform leftLeg;
    private Transform rightLeg;
    private Transform rightArm;
    private Transform leftArm;
    private Transform torso;
    private Transform helmet;
    private Transform weapon;

    public Transform weaponTransform;
    public Transform bulletTransform;
    public bool hasWeapon;

    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItemHandler>();
        if (item)
        {
            Item _item = new Item(item.groundItem.item);
            InventorySlot newSlot = inventory.AddItemFromFieldToInventory(_item, 1);
            if (newSlot != null)
            {
                Destroy(other.gameObject);
            }
            gridController.AddItem(_item, itemGrid, newSlot);

        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
        boneCombiner = new BoneCombiner(gameObject);
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
        InventoryUI = PlayerInventory.playerInventory.inventoryUI;
        EquipmentUI = PlayerInventory.playerInventory.equipmentUI;
    }

    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, " Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                        }
                    }
                }

                for (int i = 0; i < attributes.Length; i++)
                {
                    switch (attributes[i].type)
                    {
                        case Attributes.Stealth:
                            PlayerWalkState walkState = (PlayerWalkState)this.gameObject.GetComponent<PlayerStateMachine>().states[PlayerStateEnum.WALK];
                            walkState.speed = 5 + attributes[i].value.ModifiedValue;
                            break;
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.EQUIPMENT:
                            Destroy(weapon.gameObject);
                            hasWeapon = false;
                            break;
                        case ItemType.LEFT_LEG:
                            Destroy(leftLeg.gameObject);
                            break;
                        case ItemType.RIGHT_LEG:
                            Destroy(rightLeg.gameObject);
                            break;
                        case ItemType.LEFT_ARM:
                            if (leftArm != null)
                            {
                                Destroy(leftArm.gameObject);
                            }
                            break;
                        case ItemType.RIGHT_ARM:
                            Destroy(rightArm.gameObject);
                            break;
                        case ItemType.TORSO:
                            Destroy(torso.gameObject);
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
        }
    }

    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
        {
            return;
        }
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, " Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                }

                for (int i = 0; i < attributes.Length; i++)
                {
                    switch(attributes[i].type)
                    {
                        case Attributes.Stealth:
                            PlayerWalkState walkState = (PlayerWalkState) this.gameObject.GetComponent<PlayerStateMachine>().states[PlayerStateEnum.WALK];
                            walkState.speed = 5 + attributes[i].value.ModifiedValue;
                            break;
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.EQUIPMENT:
                            weapon = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform).transform;
                            hasWeapon = true;
                            break;
                        case ItemType.LEFT_LEG:
                            leftLeg = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                        case ItemType.RIGHT_LEG:
                            rightLeg = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                        case ItemType.LEFT_ARM:
                            leftArm = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                        case ItemType.RIGHT_ARM:
                            rightArm = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                        case ItemType.TORSO:
                            torso = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hi");
            playerLocation.location = transform.position;
            inventory.Save();
            equipment.Save();
            playerLocation.Save();
            worldItemList.WorldItems.groundItems.Clear();
            worldItemList.WorldItems.itemPositions.Clear();
            saveItemData.Invoke();
            StartCoroutine(SaveWorldItems());

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            inventory.Load();
            for (int i = 0; i < inventory.Container.slots.Length; i++)
            {
                if (inventory.Container.slots[i].item.Id > -1)
                {
                    gridController.AddItem(inventory.Container.slots[i].item, itemGrid, inventory.Container.slots[i]);
                }
            }
            equipment.Load();
            playerLocation.Load();
            worldItemList.Load();
            gameObject.transform.position = playerLocation.location;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryUI.gameObject.activeSelf)
            {
                Time.timeScale = 1f;
                InventoryUI.gameObject.SetActive(false);
                EquipmentUI.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                InventoryUI.gameObject.SetActive(true);
                EquipmentUI.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator SaveWorldItems()
    {
        yield return new WaitForSeconds(0.5f);
        worldItemList.Save();
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        worldItemList.WorldItems.groundItems.Clear();
        worldItemList.WorldItems.itemPositions.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;
    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
