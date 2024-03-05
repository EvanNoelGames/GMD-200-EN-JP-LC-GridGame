using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public List<ItemData> items;
    [SerializeField] private GameManager gameManager;

    public ItemData equippedWeapon;
    public ItemData equippedArmor;
    public ItemData equippedSpecial;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemIndex itemIndex;

    private void Awake()
    {
        AddItem(itemIndex.copperSword);
        EquipWeapon(items[0]);
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

    public void EquipSpecial(ItemData newArmor)
    {
        equippedSpecial = newArmor;
        inventoryUI.UpdateInventory(3);
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
            if (itemTypeSelection < 0.5f)
            {
                // weapon
                float itemSelection = Random.value;

                if (itemSelection < 0.5f && !CheckIfPlayerHasItem(itemIndex.ironSword))
                {
                    AddItem(itemIndex.ironSword);
                    DisplayNewItem(itemIndex.ironSword);
                }
                else if (!CheckIfPlayerHasItem(itemIndex.goldSword))
                {
                    AddItem(itemIndex.goldSword);
                    DisplayNewItem(itemIndex.goldSword);
                }
                else
                {
                    Debug.Log("Nothing was found");
                }
            }
            else if (itemTypeSelection < 0.9f)
            {
                // armor
                Debug.Log("Armor");
            }
            else
            {
                // special
                Debug.Log("Special");
            }
        }
    }

    private void DisplayNewItem(ItemData newItem)
    {

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
        return false;
    }
}
