using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{

    public const float tileSizeWidth = 247;
    public const float tileSizeHeight = 360;

    InventoryItem[,] inventoryItemSlot;

    [SerializeField]
    int gridSizeWidth;
    [SerializeField]
    int gridSizeHeight;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
        gameObject.SetActive(false);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = -(mousePosition.y - rectTransform.position.y);

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT))
        {
            return false;
        }

        if (!OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem, inventoryItem.itemData.Lshape, inventoryItem.rotated))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        return PlaceItem(inventoryItem, posX, posY);
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemSlot[posX, posY] = inventoryItem;

        for (int i = 0; i < inventoryItem.WIDTH; i++)
        {
            for (int j = 0; j < inventoryItem.HEIGHT; j++)
            {
                if (!inventoryItem.rotated)
                {
                    if (!inventoryItem.itemData.Lshape || !(i < inventoryItem.WIDTH - 1 && j > 0))
                    {
                        inventoryItemSlot[posX + i, posY + j] = inventoryItem;
                        Debug.Log(new Vector2(posX + i, posY + j));
                    }
                } else
                {
                    if (!inventoryItem.itemData.Lshape || !(i > 0 && j > 0))
                    {
                        inventoryItemSlot[posX + i, posY + j] = inventoryItem;
                        Debug.Log(new Vector2(posX + i, posY + j));
                    }
                }
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
        return true;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem, bool Lshape, bool rotated)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!rotated)
                {
                    if (!Lshape || (!(i < width - 1 && j > 0)))
                    {
                        if (inventoryItemSlot[posX + i, posY + j] != null)
                        {
                            if (overlapItem == null)
                            {
                                overlapItem = inventoryItemSlot[posX + i, posY + j];
                            }
                            else
                            {
                                if (overlapItem != inventoryItemSlot[posX + i, posY + j])
                                {
                                    return false;
                                }
                            }
                        }
                    }
                } else
                {
                    if (!Lshape || (!(i > 0 && j > 0)))
                    {
                        if (inventoryItemSlot[posX + i, posY + j] != null)
                        {
                            if (overlapItem == null)
                            {
                                overlapItem = inventoryItemSlot[posX + i, posY + j];
                            }
                            else
                            {
                                if (overlapItem != inventoryItemSlot[posX + i, posY + j])
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemSlot[posX + i, posY + j] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);
        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int i = 0; i < item.WIDTH; i++)
        {
            for (int j = 0; j < item.HEIGHT; j++)
            {
                if (!item.rotated)
                {
                    if (!item.itemData.Lshape || !(i < item.WIDTH - 1 && j > 0))
                    {
                        inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                    }
                } else
                {
                    if (!item.itemData.Lshape || !(i > 0 && j > 0))
                    {
                        inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                    }
                }
            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        
        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }
        
        
        return true;
    }
}
