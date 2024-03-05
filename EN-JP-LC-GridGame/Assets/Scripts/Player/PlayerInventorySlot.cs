using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySlot : MonoBehaviour
{
    [SerializeField] private ItemData item;
    public ItemData Item => item;

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
        spriteRenderer.sprite = item.sprite;
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

            if (Input.GetMouseButtonDown(0) && !equippedItem)
            {
                if (item.itemType == ItemData.ItemType.weapon)
                {
                    playerInventory.EquipWeapon(item);
                }
                else if (item.itemType == ItemData.ItemType.armor)
                {
                    playerInventory.EquipArmor(item);
                }
                else if (item.itemType == ItemData.ItemType.special)
                {
                    playerInventory.EquipSpecial(item);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        inventoryUI.itemText.text = "";
    }
}
