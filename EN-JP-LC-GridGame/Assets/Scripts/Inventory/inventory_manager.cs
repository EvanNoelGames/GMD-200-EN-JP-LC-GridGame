using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory_manager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated; 
  
    void Update()
    {
        if (Input.GetKeyDown("i") && menuActivated) 
        {
            Time.timeScale = 1; 
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown("i") && !menuActivated)
        {
            Time.timeScale = 0; 
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        Debug.Log("item name = " + itemName + "quantity = " +  quantity + "itemSprite = " + itemSprite);
    }
}
