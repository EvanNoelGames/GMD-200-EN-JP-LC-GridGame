using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerInventorySlot inventorySlotPrefab;
    [SerializeField] private PlayerInventory playerInventory;

    private bool doneLoading = false;
    public bool DoneLoading => doneLoading;

    private List<ItemData> itemData;
    public List<PlayerInventorySlot> playerInventorySlots;
    public List<PlayerInventorySlot> playerInventoryHolders;

    private ItemData equippedWeapon;
    private ItemData equippedArmor;
    private ItemData equippedSpecial;

    private float padding = 2.5f;

    private bool showing = false;

    private SpriteRenderer spriteRenderer;

    private Color equippedColor = Color.white;
    private Color unequippedColor = new Color(1, 1, 1, 0.25f);

    [SerializeField] public TextMeshProUGUI itemText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showing)
            {
                spriteRenderer.enabled = false;
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventorySlots[i].Hide();
                }
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventoryHolders[i].Hide();
                }
                showing = false;
                itemText.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventorySlots[i].Show();
                }
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventoryHolders[i].Show();
                }
                showing = true;
                itemText.enabled = true;
            }
        }
    }

    private void InitInventory()
    {
        for (int i = 0; i < gameManager.InventorySlots; i++)
        {
            PlayerInventorySlot newInventorySlot = Instantiate(inventorySlotPrefab, transform);

            if (i >= gameManager.InventorySlots / 2)
            {
                newInventorySlot.transform.localPosition = new Vector3((-3 * padding) + ((i - (gameManager.InventorySlots / 2)) * padding), -1.5f, -3);
            }
            else
            {
                newInventorySlot.transform.localPosition = new Vector3((-3 * padding) + (i * padding), 1.5f, -3);
            }
            newInventorySlot.name = $"slot_{i}";
            newInventorySlot.playerInventory = playerInventory;
            newInventorySlot.inventoryUI = this;
            newInventorySlot.Hide();
            playerInventorySlots.Add(newInventorySlot);
        }

        for (int i = 0; i < gameManager.InventorySlots; i++)
        {
            PlayerInventorySlot newInventorySlot = Instantiate(inventorySlotPrefab, transform);

            if (i >= gameManager.InventorySlots / 2)
            {
                newInventorySlot.transform.localPosition = new Vector3((-3 * padding) + ((i - (gameManager.InventorySlots / 2)) * padding), -1.5f, -2);
            }
            else
            {
                newInventorySlot.transform.localPosition = new Vector3((-3 * padding) + (i * padding), 1.5f, -2);
            }
            newInventorySlot.name = $"sprite_slot_{i}";
            newInventorySlot.Hide();
            playerInventoryHolders.Add(newInventorySlot);
        }
        doneLoading = true;
    }

    // 0 = no new equip, 1 = weapon, 2 = armor, 3 = special
    public void UpdateInventory(int newEquipType)
    {
        itemData = playerInventory.items;

        for (int i = 0; i < itemData.Count; i++)
        {
            if (playerInventorySlots[i].equippedItem == true && playerInventorySlots[i].Item.itemType == ItemData.ItemType.weapon && newEquipType == 1)
            {
                playerInventorySlots[i].equippedItem = false;
            }
            else if (playerInventorySlots[i].equippedItem == true && playerInventorySlots[i].Item.itemType == ItemData.ItemType.armor && newEquipType == 2)
            {
                playerInventorySlots[i].equippedItem = false;
            }
            else if (playerInventorySlots[i].equippedItem == true && playerInventorySlots[i].Item.itemType == ItemData.ItemType.special && newEquipType == 3)
            {
                playerInventorySlots[i].equippedItem = false;
            }
            playerInventorySlots[i].spriteRenderer.color = unequippedColor;
        }

        equippedWeapon = playerInventory.equippedWeapon;
        equippedArmor = playerInventory.equippedArmor;
        equippedSpecial = playerInventory.equippedSpecial;

        for (int i = 0; i < itemData.Count; i++)
        {
            playerInventorySlots[i].SetItem(itemData[i]);

            if (itemData[i] == equippedWeapon)
            {
                playerInventorySlots[i].spriteRenderer.color = equippedColor;
                playerInventorySlots[i].equippedItem = true;
            }
            else if (itemData[i] == equippedArmor)
            {
                playerInventorySlots[i].spriteRenderer.color = equippedColor;
                playerInventorySlots[i].equippedItem = true;
            }
            else if (itemData[i] == equippedSpecial)
            {
                playerInventorySlots[i].spriteRenderer.color = equippedColor;
                playerInventorySlots[i].equippedItem = true;
            }
        }
    }
}
