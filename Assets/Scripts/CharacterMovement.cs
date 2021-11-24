using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Board Panels")]
    [SerializeField]private GameObject m_Panels;
    private GameObject[] m_AllPanels;

    private Animator m_Animator;

    [SerializeField]private Transform m_PlayerCamera;

    [Header("Panel Info")]
    private Vector3 m_NextPanel;
    private Vector3 m_NextLocation;

    private Vector3 m_FacingDirection;

    private Rigidbody m_RB;

    private Direction m_Direction;

    private int m_PlayerPanelLocation;
    private int m_SpacesToMove;

    public float moveSpeed = 0.5f;
    [SerializeField] private float m_turnSpeed = 3f;

    private bool m_isMoving;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        //Putting panels into an array so player can navigate based on panel index
        m_AllPanels = new GameObject[m_Panels.transform.childCount];

        m_isMoving = false;

        //Loading all panels into an array
        for(int i = 0; i < m_AllPanels.Length; ++i)
        {
            m_AllPanels[i] = m_Panels.transform.GetChild(i).gameObject;
        }

        //Setting up initial player values
        m_PlayerPanelLocation = 0;
        m_Direction = Direction.Forwards;
        Vector3 startPos = new Vector3(75, 5, 97.5f);
    }

    private void Update()
    {
        //If condition to stop any other input happening to player while moving
        if (!m_isMoving)
        {
            m_Animator.SetBool("Moving", false);

            #region Moving with random number to emulate dice

            //Using space as input to "roll" the dice
            if(Input.GetKeyDown(KeyCode.Space) && m_SpacesToMove == 0)
            {
                m_SpacesToMove = Random.Range(1, 6);

                Debug.Log("You rolled a: " + m_SpacesToMove);
            }

            SetPlayerToMove();
           
            #endregion
        }


        //Move player
        #region Movment
        if (m_isMoving)
        {
            //Make player face the direction they are moving
            m_FacingDirection = Vector3.ProjectOnPlane((m_NextPanel - transform.position), Vector3.up).normalized;
            if (m_FacingDirection != Vector3.zero)
            {
                Quaternion faceDir = Quaternion.LookRotation(m_FacingDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, faceDir, m_turnSpeed);
            }

            m_RB.position = Vector3.MoveTowards(m_RB.position, m_NextLocation, moveSpeed * Time.deltaTime);

            if (m_RB.position == m_NextLocation)
            {
                m_isMoving = false;
                m_SpacesToMove--;

                //Updating the players location
                if (m_Direction == Direction.Forwards)
                {
                    m_PlayerPanelLocation++;
                }
                else
                {
                    m_PlayerPanelLocation--;
                }
            }     
        }
        #endregion
    }

    private void SetPlayerToMove()
    {
        if (m_SpacesToMove > 0)
        {

            //Checking if player can continue to move forwards
            if (m_PlayerPanelLocation < m_AllPanels.Length)
            {
                m_isMoving = true;
                m_Animator.SetBool("Moving", true);

                //Determinig the next panel the player moves to based on the direction they need to go
                if (m_Direction == Direction.Forwards)
                {
                    m_NextPanel = m_AllPanels[m_PlayerPanelLocation].transform.position;
                }
                else
                {
                    m_NextPanel = m_AllPanels[m_PlayerPanelLocation - 2].transform.position;
                }
                m_NextLocation = new Vector3(m_NextPanel.x, gameObject.transform.position.y, m_NextPanel.z);
            }
            else
            {
                //Stopping player if they are no more panels to progress on

                Debug.Log("Can't move anymore spaces");
                Debug.Log("Stopped at panel " + (m_PlayerPanelLocation));
                m_SpacesToMove = 0;
                m_isMoving = false;
            }
        }
        else
        {
            if (m_PlayerPanelLocation > 1)
            {
                PanelCheck(m_AllPanels[m_PlayerPanelLocation - 1]);
            }

            Debug.Log("Now at panel " + m_PlayerPanelLocation);

            //Setting the player to face forwards relative to the board once they have stopped moving
            if (m_PlayerPanelLocation < m_AllPanels.Length)
            {
                m_FacingDirection = Vector3.ProjectOnPlane(m_AllPanels[m_PlayerPanelLocation].transform.position - transform.position, Vector3.up).normalized;
            }
            Quaternion faceDirection = Quaternion.LookRotation(m_FacingDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, faceDirection, m_turnSpeed);
        }

    }

    /// <summary>
    /// Check the panel the player has stopped on
    /// </summary>
    /// <param name="panel">Current panel player is located on</param>
    private void PanelCheck(GameObject panel)
    {
        switch (panel.tag)
        {
            case "Plus3":
                m_SpacesToMove += 3;
                m_Direction = Direction.Forwards;
                break;
            case "Plus4":
                m_SpacesToMove += 4;
                m_Direction = Direction.Forwards;
                break;
            case "Plus5":
                m_SpacesToMove += 5;
                m_Direction = Direction.Forwards;
                break;
            case "Minus1":
                m_SpacesToMove += 1;
                m_Direction = Direction.Backwards;
                break;
            case "Minus2":
                m_SpacesToMove += 2;
                m_Direction = Direction.Backwards;
                break;
            case "Minus4":
                m_SpacesToMove += 4;
                m_Direction = Direction.Backwards;
                break;
            case "Minus6":
                m_SpacesToMove += 6;
                m_Direction = Direction.Backwards;
                break;
            default:
                m_Direction = Direction.Forwards;
                break;
        }
    }

    /// <summary>
    /// Possible facing directions for the player
    /// </summary>
    public enum Direction
    {
        Forwards,
        Backwards
    }
}
