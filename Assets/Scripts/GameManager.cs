using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Players;
    [SerializeField] private GameObject m_PlayerPlaceholder;
    [SerializeField] private GameObject m_Board;
    [SerializeField] private GameObject m_TurnStartScreen;
    [SerializeField] private GameObject m_RollNumberScreen;
    [SerializeField] private GameObject m_SpacesIndicator;
    [SerializeField] private GameObject m_EndGameScreen;
    [SerializeField] private GameObject m_PauseScreen;
    [SerializeField] private GameObject m_Camera;

    [SerializeField] private GameObject m_NumberRoll;
    [SerializeField] private GameObject m_MultipliedNumberRoll;

    private GameObject[] m_AllPlayers;
    private GameObject[] m_Placeholders;
    private GameObject[] m_Panels;

    [SerializeField] private Text m_PlayerTurnText;
    [SerializeField] private Text m_RemainingSpaces;
    [SerializeField] private Text m_SpacesLeft;

    [SerializeField] private AudioSource m_PanelSFX;
    

    private int playerTurn;
    private int m_SpacesToGo;
    private int m_direction;

    private bool m_CanChangeTurn;
    private bool m_Pause;

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
        playerTurn = 0;
        m_direction = 1;
        m_GameState = MainGameStates.TurnStart;
        m_CanChangeTurn = true;

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
        if (!m_Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_Pause = true;
            }
            else
            {
                m_PauseScreen.SetActive(false);

                switch (m_GameState)
                {

                    case MainGameStates.TurnStart:

                        m_Camera.GetComponent<CameraMovement>().target = m_AllPlayers[playerTurn].transform.GetChild(m_AllPlayers[playerTurn].transform.childCount - 1).transform;

                        m_PlayerTurnText.text = m_AllPlayers[playerTurn].name + "'s Turn";
                        m_TurnStartScreen.SetActive(true);
                        m_CanChangeTurn = true;


                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            m_Placeholders[playerTurn].SetActive(false);
                            m_AllPlayers[playerTurn].SetActive(true);
                            StartCoroutine(ChangeMainGameState(0, m_GameState, MainGameStates.RollDice));
                        }

                        break;
                    case MainGameStates.RollDice:

                        CharacterMovement currentCharacterScrpt = m_AllPlayers[playerTurn].GetComponent<CharacterMovement>();

                        if (currentCharacterScrpt.multiplier > 1)
                        {
                            m_NumberRoll.SetActive(false);
                            m_MultipliedNumberRoll.SetActive(true);

                            m_MultipliedNumberRoll.transform.GetChild(0).GetComponent<Text>().text = (Random.Range(1, 7).ToString()) + " X " + currentCharacterScrpt.multiplier.ToString();
                        }
                        else
                        {
                            m_MultipliedNumberRoll.SetActive(false);
                            m_NumberRoll.transform.GetChild(0).GetComponent<Text>().text = (Random.Range(1, 7)).ToString();
                        }

                        if (m_SpacesToGo == 0)
                        {
                            m_RollNumberScreen.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            m_SpacesToGo = Random.Range(1, 7) * currentCharacterScrpt.multiplier;
                        }

                        if (m_SpacesToGo > 0)
                        {
                            m_NumberRoll.transform.GetChild(0).GetComponent<Text>().text = m_SpacesToGo.ToString();
                            StartCoroutine(ChangeMainGameState(1, m_GameState, MainGameStates.Move));
                        }

                        break;

                    case MainGameStates.Move:

                        CharacterMovement currentCharacterScript = m_AllPlayers[playerTurn].GetComponent<CharacterMovement>();

                        m_SpacesIndicator.SetActive(true);
                        m_SpacesLeft.text = (m_SpacesToGo * m_direction).ToString();
                        m_RemainingSpaces.text = "Remaining Spaces: " + (m_Panels.Length - 1 - currentCharacterScript.currentPanel).ToString();

                        if (currentCharacterScript.currentPanel == m_Panels.Length - 1)
                        {
                            m_SpacesToGo = 0;
                        }

                        if (m_SpacesToGo > 0 && currentCharacterScript.moving == false)
                        {
                            currentCharacterScript.SetPanelToGoTo(m_Panels[currentCharacterScript.currentPanel + m_direction]);

                            if (currentCharacterScript.currentPanel < m_Panels.Length - 1 && m_SpacesToGo == 1)
                            {
                                int nextPanelPos = currentCharacterScript.currentPanel + m_direction + 1;

                                if (nextPanelPos > m_Panels.Length - 1)
                                {
                                    nextPanelPos = m_Panels.Length - 1;
                                }


                                currentCharacterScript.SetNextPanel(m_Panels[nextPanelPos]);
                            }

                            currentCharacterScript.moving = true;

                        }

                        if (m_SpacesToGo == 0 && currentCharacterScript.turning == false)
                        {
                            m_direction = 1;
                            currentCharacterScript.multiplier = 1;

                            m_NumberRoll.SetActive(true);
                            m_MultipliedNumberRoll.SetActive(false);

                            StartCoroutine(ChangeMainGameState(0.25f, m_GameState, MainGameStates.CheckPanel));
                        }

                        break;
                    case MainGameStates.CheckPanel:

                        int currentPlayerPanel = m_AllPlayers[playerTurn].GetComponent<CharacterMovement>().currentPanel;
                        switch (m_Panels[currentPlayerPanel].tag)
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
                        StartCoroutine(ChangeMainGameState(0, m_GameState, MainGameStates.ActionBasedOnPanel));

                        break;
                    case MainGameStates.ActionBasedOnPanel:

                        CharacterMovement currentCharacterScr = m_AllPlayers[playerTurn].GetComponent<CharacterMovement>();

                        switch (m_PanelType)
                        {
                            case PanelLandedOn.Blank:
                                StartCoroutine(ChangeMainGameState(0, m_GameState, MainGameStates.NextPlayer));
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
                                m_EndGameScreen.SetActive(true);

                                m_AllPlayers[playerTurn].GetComponent<CharacterMovement>().StopRunAnimaion();
                                m_AllPlayers[playerTurn].GetComponent<CharacterMovement>().StartVictoryAnimation();
                                m_Camera.GetComponent<CameraMovement>().m_KeepDistance = -3f;

                                if (Input.GetKeyDown(KeyCode.Return))
                                {
                                    BackToMenu();
                                }
                                break;
                        }

                        break;

                    case MainGameStates.NextPlayer:

                        m_Placeholders[playerTurn].SetActive(true);
                        m_AllPlayers[playerTurn].SetActive(false);

                        if (m_CanChangeTurn)
                        {
                            playerTurn++;
                            m_CanChangeTurn = false;
                        }

                        if (playerTurn > m_Players.transform.childCount - 1)
                        {
                            playerTurn -= m_Players.transform.childCount;
                        }

                        StartCoroutine(ChangeMainGameState(0.5f, m_GameState, MainGameStates.TurnStart));

                        break;
                }
            }
        }
        else
        {
            m_PauseScreen.SetActive(true);
        }

    }

    public void MovedOneSpace()
    {
        m_SpacesToGo--;
        m_PanelSFX.Play();
    }

    public int GetSpacesToMove()
    {
        return m_SpacesToGo;
    }

    public int GetDirection()
    {
        return m_direction;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UnPause()
    {
        m_Pause = false;
    }

    public void MoveForwardsPanels(int spaces)
    {
        m_direction = 1;
        m_SpacesToGo = spaces;
        StartCoroutine(ChangeMainGameState(0, m_GameState, MainGameStates.Move));
    }

    public void MoveBackwardsPanels(int spaces)
    {
        m_direction = - 1;
        m_SpacesToGo = spaces;
        StartCoroutine(ChangeMainGameState(0, m_GameState, MainGameStates.Move));
    }


    IEnumerator ChangeMainGameState(float delay, MainGameStates currentState, MainGameStates targetState)
    {
        yield return new WaitForSeconds(delay);

        switch (currentState)
        {
            case MainGameStates.TurnStart:
                m_TurnStartScreen.SetActive(false);
                m_GameState = targetState;
                break;
            case MainGameStates.RollDice:
                m_RollNumberScreen.SetActive(false);
                m_GameState = targetState;
                break;
            case MainGameStates.Move:
                m_SpacesIndicator.SetActive(false);
                m_GameState = targetState;
                break;
            case MainGameStates.CheckPanel:
                m_GameState = targetState;
                break;
            case MainGameStates.ActionBasedOnPanel:
                m_GameState = targetState;
                break;
            case MainGameStates.NextPlayer:
                m_GameState = targetState;
                break;
        }
    }

}