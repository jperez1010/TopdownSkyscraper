using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New consumable object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.CONSUMABLE;
    }
}
