using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerInventorySlot inventorySlotPrefab;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private BattleSystem battleSystem;

    private bool doneLoading = false;
    public bool DoneLoading => doneLoading;

    private List<ItemData> itemData;
    public List<PlayerInventorySlot> playerInventorySlots;

    public PlayerInventorySlot equippedWeaponSlot;
    public PlayerInventorySlot equippedArmorSlot;
    public PlayerInventorySlot equippedSpecialSlot;

    public List<PlayerInventorySlot> playerInventoryHolders;

    [SerializeField] private ItemData equippedWeapon;
    [SerializeField] private ItemData equippedArmor;
    [SerializeField] private ItemData equippedSpecial;

    private float padding = 2.5f;

    private bool showing = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField] public TextMeshProUGUI itemText;
    [SerializeField] public TextMeshProUGUI weaponText;
    [SerializeField] public TextMeshProUGUI armorText;
    [SerializeField] public TextMeshProUGUI specialText;
    [SerializeField] public TextMeshProUGUI statsText;
    [SerializeField] public Camera cam;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitInventory();
    }

    private void Update()
    {
        statsText.text = "DMG: " + equippedWeapon.damage + " DEFENCE: " + equippedArmor.defence;
        if (cam.isActiveAndEnabled)
        {
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -5);
        }
        else
        {
            transform.position = new Vector3(0, 0, -5);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showing)
            {
                spriteRenderer.enabled = false;
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventorySlots[i].Hide();
                }
                for (int i = 0; i < playerInventoryHolders.Count; i++)
                {
                    playerInventoryHolders[i].Hide();
                }

                equippedWeaponSlot.Hide();
                equippedArmorSlot.Hide();
                equippedSpecialSlot.Hide();

                showing = false;
                itemText.enabled = false;
                weaponText.enabled = false;
                armorText.enabled = false;
                specialText.enabled = false;
                statsText.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
                for (int i = 0; i < playerInventorySlots.Count; i++)
                {
                    playerInventorySlots[i].Show();
                }
                for (int i = 0; i < playerInventoryHolders.Count; i++)
                {
                    playerInventoryHolders[i].Show();
                }

                equippedWeaponSlot.Show();
                equippedArmorSlot.Show();
                equippedSpecialSlot.Show();

                showing = true;
                itemText.enabled = true;
                weaponText.enabled = true;
                armorText.enabled = true;
                specialText.enabled = true;
                statsText.enabled = true;
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

        equippedWeaponSlot = Instantiate(inventorySlotPrefab, transform);
        equippedWeaponSlot.transform.localPosition = new Vector3(7, 3, -2);
        equippedWeaponSlot.name = "weapon_slot";
        equippedWeaponSlot.playerInventory = playerInventory;
        equippedWeaponSlot.inventoryUI = this;
        equippedWeaponSlot.Hide();
        equippedArmorSlot = Instantiate(inventorySlotPrefab, transform);
        equippedArmorSlot.transform.localPosition = new Vector3(7, 0, -2);
        equippedArmorSlot.name = "armor_slot";
        equippedArmorSlot.playerInventory = playerInventory;
        equippedArmorSlot.inventoryUI = this;
        equippedArmorSlot.Hide();
        equippedSpecialSlot = Instantiate(inventorySlotPrefab, transform);
        equippedSpecialSlot.transform.localPosition = new Vector3(7, -3, -2);
        equippedSpecialSlot.name = "special_slot";
        equippedSpecialSlot.playerInventory = playerInventory;
        equippedSpecialSlot.inventoryUI = this;
        equippedSpecialSlot.Hide();

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

        PlayerInventorySlot equippedWeaponSlotHolder = Instantiate(inventorySlotPrefab, transform);
        equippedWeaponSlotHolder.transform.localPosition = new Vector3(7, 3, -1);
        equippedWeaponSlotHolder.name = "weapon_slot_holder";
        equippedWeaponSlotHolder.GetComponent<BoxCollider2D>().enabled = false;
        equippedWeaponSlotHolder.Hide();
        PlayerInventorySlot equippedArmorSlotHolder = Instantiate(inventorySlotPrefab, transform);
        equippedArmorSlotHolder.transform.localPosition = new Vector3(7, 0, -1);
        equippedArmorSlotHolder.name = "armor_slot_holder";
        equippedArmorSlotHolder.GetComponent<BoxCollider2D>().enabled = false;
        equippedArmorSlotHolder.Hide();
        PlayerInventorySlot equippedSpecialSlotHolder = Instantiate(inventorySlotPrefab, transform);
        equippedSpecialSlotHolder.transform.localPosition = new Vector3(7, -3, -1);
        equippedSpecialSlotHolder.name = "special_slot_holder";
        equippedSpecialSlotHolder.GetComponent<BoxCollider2D>().enabled = false;
        equippedSpecialSlotHolder.Hide();

        playerInventoryHolders.Add(equippedWeaponSlotHolder);
        playerInventoryHolders.Add(equippedArmorSlotHolder);
        playerInventoryHolders.Add(equippedSpecialSlotHolder);

        playerInventory.StartingItems();
        doneLoading = true;
    }

    // 0 = no new equip, 1 = weapon, 2 = armor, 3 = special, 4 = health
    public void UpdateInventory(int newEquipType)
    {
        itemData = playerInventory.items;

        // remove armor buffs first upon change
        if (equippedArmor != null )
        {
            playerStats.playerMaxHealth -= equippedArmor.defence;
        }

        for (int i = 0; i < itemData.Count; i++)
        {
            playerInventorySlots[i].SetItem(itemData[i]);

            if (itemData[i] == playerInventory.itemToEat && newEquipType == 4)
            {
                playerStats.playerHealth += playerInventory.itemToEat.healing;
                playerInventory.items.Remove(playerInventory.itemToEat);
                playerInventory.itemToEat = null;
                playerInventorySlots[i].ClearItem();
            }
        }

        equippedWeapon = playerInventory.equippedWeapon;
        equippedArmor = playerInventory.equippedArmor;
        equippedSpecial = playerInventory.equippedSpecial;

        equippedWeaponSlot.item = equippedWeapon;
        equippedWeaponSlot.equippedItem = true;
        equippedWeaponSlot.UpdateSprite();
        equippedArmorSlot.item = equippedArmor;
        equippedArmorSlot.equippedItem = true;
        equippedArmorSlot.UpdateSprite();
        equippedSpecialSlot.item = equippedSpecial;
        equippedSpecialSlot.equippedItem = true;
        equippedSpecialSlot.UpdateSprite();

        // update item defence
        if (equippedArmor != null)
        {
            playerStats.playerMaxHealth += equippedArmor.defence;
            playerStats.ClampHealth();
        }

        // if you change weapon or armor mid-battle then update the player unit
        if (battleSystem.isActiveAndEnabled)
        {
            battleSystem.playerUnit.damage = equippedWeapon.damage;
            battleSystem.playerUnit.maxHP = playerStats.playerMaxHealth;
            battleSystem.playerUnit.currentHP = playerStats.playerHealth;
        }

        if (newEquipType == 1)
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                if (itemData[i] == playerInventory.equippedWeapon)
                {
                    playerInventorySlots[i].ClearItem();
                    playerInventory.items.Remove(itemData[i]);
                }
            }
        }
        else if (newEquipType == 2)
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                if (itemData[i] == playerInventory.equippedArmor)
                {
                    playerInventorySlots[i].ClearItem();
                    playerInventory.items.Remove(itemData[i]);
                }
            }
        }
        else if (newEquipType == 3)
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                if (itemData[i] == playerInventory.equippedSpecial)
                {
                    playerInventorySlots[i].ClearItem();
                    playerInventory.items.Remove(itemData[i]);
                }
            }
        }

        // move items down the line to fill blank slots
        for (int i = 0; i < playerInventorySlots.Count; i++)
        {
            bool moreItemsAfterNull = false;
            if (playerInventorySlots[i].item == null)
            {
                for (int j = i; j < playerInventorySlots.Count; j++)
                {
                    if (playerInventorySlots[j].item != null)
                    {
                        moreItemsAfterNull = true;
                    }
                }
                if (moreItemsAfterNull)
                {
                    playerInventorySlots[i].item = playerInventorySlots[i + 1].item;
                    playerInventorySlots[i + 1].item = null;
                    playerInventorySlots[i].UpdateSprite();
                    playerInventorySlots[i + 1].ClearItem();
                }
            }
        }
        battleSystem.UpdatePlayerHP();
    }
}
