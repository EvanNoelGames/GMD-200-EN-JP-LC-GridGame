using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySlot : MonoBehaviour
{
    [SerializeField] public ItemData item;

    public bool equippedItem = false;

    [SerializeField] private Sprite emptySlot;
    public PlayerInventory playerInventory;

    public SpriteRenderer spriteRenderer;

    public InventoryUI inventoryUI;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite()
    {
        if (item != null)
        {
            spriteRenderer.sprite = item.sprite;
        }
    }

    public void ClearItem()
    {
        item = null;
        spriteRenderer.sprite = emptySlot;
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        UpdateSprite();
    }

    public ItemData GetItem()
    {
        return item;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
    }

    private void OnMouseOver()
    {
        if (item != null)
        {
            if (!equippedItem)
            {
                inventoryUI.itemText.text = item.itemName;
            }
            else
            {
                inventoryUI.itemText.text = item.itemName + " (equipped)";
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!equippedItem)
                {
                    if (item.itemType == ItemData.ItemType.weapon)
                    {
                        playerInventory.EquipWeapon(item);
                        ClearItem();
                    }
                    else if (item.itemType == ItemData.ItemType.armor)
                    {
                        playerInventory.EquipArmor(item);
                        ClearItem();
                    }
                    else if (item.itemType == ItemData.ItemType.special)
                    {
                        playerInventory.EquipSpecial(item);
                        ClearItem();
                    }
                    else if (item.itemType == ItemData.ItemType.health)
                    {
                        playerInventory.HealthItem(item);
                    }
                }
            }
        }
        else
        {
            if (item == null)
            {
                inventoryUI.itemText.text = "";
            }
        }
    }

    private void OnMouseExit()
    {
        if (item != null)
        {
            inventoryUI.itemText.text = "";
        }
    }
}
