using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;

    private inventory_manager inventory_Manager;
    void Start()
    {
        inventory_Manager = GameObject.Find("InventoryCanvas").GetComponent<inventory_manager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inventory_Manager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }
}
