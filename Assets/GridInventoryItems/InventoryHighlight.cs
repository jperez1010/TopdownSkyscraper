using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highLighter;

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highLighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        highLighter.localPosition = pos;

    }

    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }
        highLighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posx, int posy)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posx, posy);

        highLighter.localPosition = pos;
    }

    public void Show(bool b)
    {
        highLighter.gameObject.SetActive(b);
    }
}
