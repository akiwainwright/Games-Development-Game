using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaceHolder : MonoBehaviour
{
    [SerializeField] private Transform Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        switch(Player.name)
        {
            case "Player 1":
                float offset = 1.75f;
                transform.position = Player.transform.position + (Player.transform.forward * offset) + (Player.transform.right * -offset);
                transform.forward = -Player.forward;
                break;
            case "Player 2":
                break;
            case "Player 3":
                break;
            case "Player 4":
                break;
        }
    }

}
