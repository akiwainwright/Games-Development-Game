using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Players;
    [SerializeField] private GameObject m_Board;

    private GameObject[] m_PlayerList;
    private GameObject[] m_Panels;

    private int playerTurn;
    private int m_SpacesToGo;

    enum MainGameStates
    {
        TurnStart,
        RollDice,
        Move,
        CheckPanel,
        ActionBasedOnPanel,
        NextPlayer
    }

    MainGameStates gameStates;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = 1;
        gameStates = MainGameStates.RollDice;

        m_Panels = new GameObject[m_Board.transform.childCount];
        m_PlayerList = new GameObject[m_Players.transform.childCount];

        for(int i = 0; i < m_Panels.Length; ++i)
        {
            m_Panels[i] = m_Board.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < m_PlayerList.Length; ++i)
        {
            m_PlayerList[i] = m_Players.transform.GetChild(i).gameObject;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a");
        }
        
        switch (gameStates)
        {
            case MainGameStates.TurnStart:
                break;
            case MainGameStates.RollDice:

                {
                    m_SpacesToGo = Random.Range(1, 7);
                    Debug.Log("Rolled: " + m_SpacesToGo);
                }

                if (m_SpacesToGo > 0)
                {
                    gameStates = MainGameStates.Move;
                }
                
                break;

            case MainGameStates.Move:

                m_PlayerList[playerTurn - 1].GetComponent<CharacterMovement>().SetPlayerToMove();

                break;
            case MainGameStates.CheckPanel:
                break;
            case MainGameStates.ActionBasedOnPanel:
                break;
            case MainGameStates.NextPlayer:
                break;
        }
    }
}