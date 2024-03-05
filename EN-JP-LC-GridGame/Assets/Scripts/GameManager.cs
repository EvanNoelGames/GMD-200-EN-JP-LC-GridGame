using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private int currentFloor = 1;
    private int inventorySlots = 8;
    public int CurrentFloor => currentFloor;
    public int InventorySlots => inventorySlots;


}
