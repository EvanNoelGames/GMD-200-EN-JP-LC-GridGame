using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private int currentFloor = 5;
    public int CurrentFloor => currentFloor;


}
