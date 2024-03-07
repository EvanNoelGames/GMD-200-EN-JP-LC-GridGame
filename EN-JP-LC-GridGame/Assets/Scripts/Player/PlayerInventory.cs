using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public List<ItemData> items;
    [SerializeField] private GameManager gameManager;

    public ItemData equippedWeapon;
    public ItemData equippedArmor;
    public ItemData equippedSpecial;
    public ItemData itemToEat;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemIndex itemIndex;

    [SerializeField] private NewItem newItemPrefab;
    [SerializeField] private TextMeshProUGUI exitStepCounter;
    [SerializeField] private Canvas inventoryCanvas;

    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (equippedSpecial != null)
        {
            if (equippedSpecial.itemName == "Compass" && playerMovement.RoomManager.roomType != RoomManager.Type.exit)
            {
                exitStepCounter.text = "EXIT DIRECTION: " + playerMovement.GetExitDistance();
            }
            else
            {
                exitStepCounter.text = "";
            }
        }
    }

    public void StartingItems()
    {
        AddItem(itemIndex.copperSword);
        AddItem(itemIndex.shield);
        AddItem(itemIndex.apple);
        EquipWeapon(items[0]);
        EquipArmor(items[0]);
    }

    public void EquipWeapon(ItemData newWeapon)
    {
        equippedWeapon = newWeapon;
        inventoryUI.UpdateInventory(1);
    }

    public void EquipArmor(ItemData newArmor)
    {
        equippedArmor = newArmor;
        inventoryUI.UpdateInventory(2);
    }

    public void EquipSpecial(ItemData newSpecial)
    {
        equippedSpecial = newSpecial;
        if (equippedSpecial.itemName == "Compass")
        {
            exitStepCounter.enabled = true;
        }
        else
        {
            exitStepCounter.enabled = false;
        }

        inventoryUI.UpdateInventory(3);
    }

    public void HealthItem(ItemData healthItem)
    {
        itemToEat = healthItem;
        inventoryUI.UpdateInventory(4);
    }

    public void AddItem(ItemData item)
    {
        if (items.Count > gameManager.InventorySlots)
        {
            return;
        }

        items.Add(item);
        inventoryUI.UpdateInventory(0);
    }

    public void AddRandomItemFromChest()
    {
        if (items.Count >= gameManager.InventorySlots)
        {
            // what do we do if we are out of inventory space
            Debug.Log("No more space");
        }
        else
        {
            float itemTypeSelection = Random.value;

            if (itemTypeSelection < 0.5f && equippedWeapon.name != "Dark Sword")
            {
                // weapon

                float itemSelection = Random.value;

                if (CheckIfPlayerHasItem(itemIndex.copperSword))
                {
                    EquipWeapon(itemIndex.ironSword);
                    DisplayNewItem(itemIndex.ironSword);
                }
                else if (CheckIfPlayerHasItem(itemIndex.ironSword))
                {
                    EquipWeapon(itemIndex.goldSword);
                    DisplayNewItem(itemIndex.goldSword);
                }
                else if (itemSelection >= 0.7f)
                {
                    EquipWeapon(itemIndex.darkSword);
                    DisplayNewItem(itemIndex.darkSword);
                }
            }
            else if (itemTypeSelection < 0.6f && equippedArmor.name != "Chestplate")
            {
                // armor

                EquipArmor(itemIndex.chestplate);
                DisplayNewItem(itemIndex.chestplate);
            }
            else if (itemTypeSelection < 0.9f)
            {
                // food

                if (Random.value < 0.6)
                {
                    AddItem(itemIndex.apple);
                    DisplayNewItem(itemIndex.apple);
                }
                else
                {
                    AddItem(itemIndex.applePie);
                    DisplayNewItem(itemIndex.applePie);
                }
            }
            else if (!CheckIfPlayerHasItem(itemIndex.compass))
            {
                // special

                AddItem(itemIndex.compass);
                DisplayNewItem(itemIndex.compass);
            }
            else
            {
                // give player XP
            }
        }
    }

    private void DisplayNewItem(ItemData newItem)
    {
        NewItem newItemToDisplay = Instantiate(newItemPrefab, inventoryCanvas.transform);
        newItemToDisplay.GetComponent<TextMeshProUGUI>().text = "Got " + newItem.name + "!";
    }

    private bool CheckIfPlayerHasItem(ItemData itemCheck)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (itemCheck == items[i])
            {
                return true;
            }
        }
        if (equippedWeapon == itemCheck)
        {
            return true;
        }
        if (equippedArmor == itemCheck)
        {
            return true;
        }
        if (equippedSpecial == itemCheck)
        {
            return true;
        }
        return false;
    }
}
