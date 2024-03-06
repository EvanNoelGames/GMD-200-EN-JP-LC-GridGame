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
    public ItemData itemToEat;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemIndex itemIndex;

    public void StartingItems()
    {
        AddItem(itemIndex.copperSword);
        AddItem(itemIndex.shield);
        AddItem(itemIndex.apple);
        AddItem(itemIndex.applePie);
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

    public void EquipSpecial(ItemData newArmor)
    {
        equippedSpecial = newArmor;
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
            if (itemTypeSelection < 0.5f)
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

                if (itemSelection >= 0.99f)
                {
                    EquipWeapon(itemIndex.darkSword);
                    DisplayNewItem(itemIndex.darkSword);
                }
            }
            else if (itemTypeSelection < 0.6f)
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
