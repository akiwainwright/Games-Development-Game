using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaceHolder : MonoBehaviour
{
    [SerializeField] private Transform m_Player;

    private void Start()
    {
        if(m_Player.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private float offset = 1.75f;

    private void OnEnable()
    {
        switch(m_Player.name)
        {
            case "Player 1":
                transform.position = m_Player.transform.position + (m_Player.transform.forward * offset) + (m_Player.transform.right * -offset);
                transform.forward = -m_Player.forward;
                break;
            case "Player 2":
                transform.position = m_Player.transform.position + (m_Player.transform.forward * offset) + (m_Player.transform.right * offset);
                transform.forward = -m_Player.forward;
                break;
            case "Player 3":
                transform.position = m_Player.transform.position - (m_Player.transform.forward * offset) + (m_Player.transform.right * offset);
                transform.forward = -m_Player.forward;
                break;
            case "Player 4":
                transform.position = m_Player.transform.position - (m_Player.transform.forward * offset) - (m_Player.transform.right * offset);
                transform.forward = -m_Player.forward;
                break;
        }
    }

}
