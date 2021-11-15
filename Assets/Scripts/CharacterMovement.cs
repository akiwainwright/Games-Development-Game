using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Board Panels")]
    [SerializeField]private GameObject m_Panels;
    private GameObject[] m_AllPanels;

    [SerializeField]private Transform m_PlayerCamera;

    [Header("Panel Info")]
    private Vector3 m_NextPanel;
    private Vector3 m_NextLocation;

    private Vector3 m_FacingDirection;

    private Rigidbody m_RB;

    private int m_PlayerPanelLocation;

    private int m_Direction;
    private int m_SpacesToMove;

    public float moveSpeed = 0.5f;

    private bool m_isMoving;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();

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
        m_Direction = 1;
        Vector3 startPos = new Vector3(75, 5, 97.5f);
    }

    private void Update()
    {
        //If condition to stop any other input happening to player while moving
        if (!m_isMoving)
        {
            //Determine moving and facing direction from input
            #region Determine Movement Direction
            //if (Input.GetKeyDown(KeyCode.W) && m_playerPanelLocation < allPanels.Length - 1)
            //{
            //    m_direction = 1;
            //    m_isMoving = true;
            //    m_nextPanel = allPanels[m_playerPanelLocation + 1].transform.position;
            //    m_nextLocation = new Vector3(m_nextPanel.x, gameObject.transform.position.y, m_nextPanel.z);

            //    //Make player face the direction they are moving
            //    m_RB.transform.right = new Vector3((m_nextPanel.x - m_RB.position.x), 0, (m_nextPanel.z - m_RB.position.z)).normalized;
            //}
            //else if (Input.GetKeyDown(KeyCode.S) && m_playerPanelLocation > 0)
            //{
            //    m_direction = -1;
            //    m_isMoving = true;
            //    m_nextPanel = allPanels[m_playerPanelLocation - 1].transform.position;
            //    m_nextLocation = new Vector3(m_nextPanel.x, gameObject.transform.position.y, m_nextPanel.z);

            //    //Make player face the direction they are moving
            //    m_RB.transform.right = new Vector3((m_nextPanel.x - m_RB.position.x), 0, (m_nextPanel.z - m_RB.position.z)).normalized;
            //}
            #endregion

            #region Moving with random number to emulate dice

            //Using space as input to "roll" the dice
            if(Input.GetKeyDown(KeyCode.Space) && m_SpacesToMove == 0)
            {
                m_SpacesToMove = Random.Range(1, 6);

                Debug.Log("You rolled a: " + m_SpacesToMove);
            }

            if(m_SpacesToMove > 0)
            {
                m_Direction = 1;

                //Checking if player can continue to move forwards
                if (m_PlayerPanelLocation != m_AllPanels.Length)
                {
                    m_isMoving = true;
                    m_NextPanel = m_AllPanels[m_PlayerPanelLocation].transform.position;
                    m_NextLocation = new Vector3(m_NextPanel.x, gameObject.transform.position.y, m_NextPanel.z);
                }
                else
                {
                    Debug.Log("Can't move anymore spaces");
                    Debug.Log("Stopped at panel " + (m_PlayerPanelLocation));
                    m_SpacesToMove = 0;
                    m_isMoving = false;
                }
            }
            else
            {
                m_FacingDirection = Vector3.ProjectOnPlane(m_PlayerCamera.position - transform.position, Vector3.up).normalized;
                m_RB.transform.rotation = Quaternion.LookRotation(m_FacingDirection);
            }
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
                m_RB.rotation = Quaternion.LookRotation(m_FacingDirection);
            }

            Debug.Log("Moving");
            m_RB.position = Vector3.MoveTowards(m_RB.position, m_NextLocation, moveSpeed * Time.deltaTime);

            if (m_RB.position == m_NextLocation)
            {
                m_isMoving = false;
                m_SpacesToMove--;
                m_PlayerPanelLocation += m_Direction;
                Debug.Log("Now at panel: " + m_PlayerPanelLocation);
            }     
        }
        #endregion
    }
}
