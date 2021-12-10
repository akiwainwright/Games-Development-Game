using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject GameManager;

    public delegate void DiceRoll();
    public static event DiceRoll RolledDice;



    private void Update()
    {
        
    }

}
