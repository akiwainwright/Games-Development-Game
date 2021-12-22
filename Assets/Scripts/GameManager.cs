using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Players;
    [SerializeField] private GameObject m_PlayerPlaceholder;
    [SerializeField] private GameObject m_Board;

    private GameObject[] m_AllPlayers;
    private GameObject[] m_Placeholders;
    private GameObject[] m_Panels;

    private int playerTurn;
    private int m_SpacesToGo;
    private int m_direction;


    enum MainGameStates
    {
        TurnStart,
        RollDice,
        Move,
        CheckPanel,
        ActionBasedOnPanel,
        NextPlayer
    }

    enum PanelLandedOn
    {
        Blank,
        Plus3,
        Plus4,
        Plus5,
        Minus1,
        Minus2,
        Minus4,
        Minus6,
        Multiply2,
        Multiply3,
        End
    }

    private MainGameStates m_GameState;
    private PanelLandedOn m_PanelType;

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = 1;
        m_direction = 1;
        m_GameState = MainGameStates.TurnStart;

        m_Panels = new GameObject[m_Board.transform.childCount];
        m_AllPlayers = new GameObject[m_Players.transform.childCount];
        m_Placeholders = new GameObject[m_PlayerPlaceholder.transform.childCount];

        for(int i = 0; i < m_Panels.Length; ++i)
        {
            m_Panels[i] = m_Board.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < m_AllPlayers.Length; ++i)
        {
            m_AllPlayers[i] = m_Players.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < m_Placeholders.Length; ++i)
        {
            m_Placeholders[i] = m_PlayerPlaceholder.transform.GetChild(i).gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {        
        switch (m_GameState)
        {
            case MainGameStates.TurnStart:

                Debug.Log("Press Enter to start turn");

                if(Input.GetKeyDown(KeyCode.Return))
                {
                    m_Placeholders[playerTurn - 1].SetActive(false);
                    m_AllPlayers[playerTurn - 1].SetActive(true);
                    m_GameState = MainGameStates.RollDice;
                }

                break;
            case MainGameStates.RollDice:

                CharacterMovement currentCharacterScrpt = m_AllPlayers[playerTurn - 1].GetComponent<CharacterMovement>();

                Debug.Log("Press Space To Roll The dice");

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    m_SpacesToGo = Random.Range(1, 7) * currentCharacterScrpt.multiplier;
                    Debug.Log("Rolled: " + m_SpacesToGo);
                }

                if (m_SpacesToGo > 0)
                {
                    m_GameState = MainGameStates.Move;
                }
                
                break;

            case MainGameStates.Move:

                CharacterMovement currentCharacterScript = m_AllPlayers[playerTurn - 1].GetComponent<CharacterMovement>();

                if(currentCharacterScript.currentPanel == m_Panels.Length - 1)
                {
                    m_SpacesToGo = 0;
                }

                if(m_SpacesToGo > 0 && currentCharacterScript.moving == false)
                {
                    currentCharacterScript.SetPanelToGoTo(m_Panels[currentCharacterScript.currentPanel + m_direction]);

                    if(currentCharacterScript.currentPanel < m_Panels.Length - 1 && m_SpacesToGo == 1)
                    {
                        int nextPanelPos = currentCharacterScript.currentPanel + m_direction + 1;

                        if ( nextPanelPos > m_Panels.Length - 1)
                        {
                            nextPanelPos = m_Panels.Length - 1;
                        }
                        

                        currentCharacterScript.SetNextPanel(m_Panels[nextPanelPos]);
                    }

                    currentCharacterScript.moving = true;

                }
                
                if(m_SpacesToGo == 0 && currentCharacterScript.turning == false)
                {
                    m_direction = 1;
                    currentCharacterScript.multiplier = 1;
                    m_GameState = MainGameStates.CheckPanel;
                }
                
                break;
            case MainGameStates.CheckPanel:

                int currentPlayerPanel = m_AllPlayers[playerTurn - 1].GetComponent<CharacterMovement>().currentPanel;
                switch(m_Panels[currentPlayerPanel].tag)
                {
                    case "Plus3":
                        m_PanelType = PanelLandedOn.Plus3;
                        break;
                    case "Plus4":
                        m_PanelType = PanelLandedOn.Plus4;
                        break;
                    case "Plus5":
                        m_PanelType = PanelLandedOn.Plus5;
                        break;
                    case "Minus1":
                        m_PanelType = PanelLandedOn.Minus1;
                        break;
                    case "Minus2":
                        m_PanelType = PanelLandedOn.Minus2;
                        break;
                    case "Minus4":
                        m_PanelType = PanelLandedOn.Minus4;
                        break;
                    case "Minus6":
                        m_PanelType = PanelLandedOn.Minus6;
                        break;
                    case "Times2":
                        m_PanelType = PanelLandedOn.Multiply2;
                        break;
                    case "Times3":
                        m_PanelType = PanelLandedOn.Multiply3;
                        break;
                    case "Last":
                        m_PanelType = PanelLandedOn.End;
                        break;
                    default:
                        m_PanelType = PanelLandedOn.Blank;
                        break;
                }
                m_GameState = MainGameStates.ActionBasedOnPanel;

                break;
            case MainGameStates.ActionBasedOnPanel:

                CharacterMovement currentCharacterScr = m_AllPlayers[playerTurn - 1].GetComponent<CharacterMovement>();

                switch (m_PanelType)
                {
                    case PanelLandedOn.Blank:
                        m_GameState = MainGameStates.NextPlayer;
                        break;
                    case PanelLandedOn.Plus3:
                        MoveForwardsPanels(3);
                        break;
                    case PanelLandedOn.Plus4:
                        MoveForwardsPanels(4);
                        break;
                    case PanelLandedOn.Plus5:
                        MoveForwardsPanels(5);
                        break;
                    case PanelLandedOn.Minus1:
                        MoveBackwardsPanels(1);
                        break;
                    case PanelLandedOn.Minus2:
                        MoveBackwardsPanels(2);
                        break;
                    case PanelLandedOn.Minus4:
                        MoveBackwardsPanels(4);
                        break;
                    case PanelLandedOn.Minus6:
                        MoveBackwardsPanels(6);
                        break;
                    case PanelLandedOn.Multiply2:
                        currentCharacterScr.multiplier = 2;
                        m_GameState = MainGameStates.NextPlayer;
                        break;
                    case PanelLandedOn.Multiply3:
                        currentCharacterScr.multiplier = 3;
                        m_GameState = MainGameStates.NextPlayer;
                        break;
                    case PanelLandedOn.End:
                        UnityEditor.EditorApplication.isPlaying = false;
                        break;
                }

                break;
            case MainGameStates.NextPlayer:

                Debug.Log("Next Player's Turn");
                m_Placeholders[playerTurn - 1].SetActive(true);
                m_AllPlayers[playerTurn - 1].SetActive(false);                

                playerTurn++;

                if(playerTurn > m_Players.transform.childCount)
                {
                    playerTurn -= m_Players.transform.childCount;
                }

                m_GameState = MainGameStates.TurnStart;
                break;
        }
    }

    public void MovedOneSpace()
    {
        m_SpacesToGo--;
    }

    public int GetSpacesToMove()
    {
        return m_SpacesToGo;
    }

    public int GetDirection()
    {
        return m_direction;
    }

    public void MoveForwardsPanels(int spaces)
    {
        m_direction = 1;
        m_SpacesToGo = spaces;
        m_GameState = MainGameStates.Move;
    }

    public void MoveBackwardsPanels(int spaces)
    {
        m_direction = - 1;
        m_SpacesToGo = spaces;
        m_GameState = MainGameStates.Move;
    }
}