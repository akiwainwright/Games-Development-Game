using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject panels;
    private GameObject[] allPanels;

    
    private Vector3 m_nextPanel;
    private Vector3 m_nextLocation;

    private Rigidbody m_RB;

    private int m_playerPanelLocation;
    private int m_direction;

    public float moveSpeed = 0.5f;

    private bool m_isMoving;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();

        //Putting panels into an array so player can navigate based on panel index
        allPanels = new GameObject[panels.transform.childCount];

        m_isMoving = false;

        //Loading all panels into an array
        for(int i = 0; i < allPanels.Length; ++i)
        {
            allPanels[i] = panels.transform.GetChild(i).gameObject;
        }

        //Setting up initial player values
        m_playerPanelLocation = 0;
        m_direction = 1;
        Vector3 startOffset = new Vector3(0, 2, 0);
        gameObject.transform.position = allPanels[m_playerPanelLocation].transform.localPosition + startOffset;
    }

    private void Update()
    {
        //If condition to stop any other input happening to player while moving
        if (!m_isMoving)
        {
            //Determine moving and facing direction from input
            #region Determine Movement Direction
            if (Input.GetKeyDown(KeyCode.W) && m_playerPanelLocation < allPanels.Length - 1)
            {
                m_direction = 1;
                m_isMoving = true;
                m_nextPanel = allPanels[m_playerPanelLocation + 1].transform.position;
                m_nextLocation = new Vector3(m_nextPanel.x, gameObject.transform.position.y, m_nextPanel.z);

                //Make player face the direction they are moving
                m_RB.transform.right = new Vector3((m_nextPanel.x - m_RB.position.x), 0, (m_nextPanel.z - m_RB.position.z)).normalized;
            }
            else if (Input.GetKeyDown(KeyCode.S) && m_playerPanelLocation > 0)
            {
                m_direction = -1;
                m_isMoving = true;
                m_nextPanel = allPanels[m_playerPanelLocation - 1].transform.position;
                m_nextLocation = new Vector3(m_nextPanel.x, gameObject.transform.position.y, m_nextPanel.z);

                //Make player face the direction they are moving
                m_RB.transform.right = new Vector3((m_nextPanel.x - m_RB.position.x), 0, (m_nextPanel.z - m_RB.position.z)).normalized;
            }
            #endregion
        }
        

        //Move player
        #region Movment
        if (m_isMoving)
        {
            Debug.Log("Moving");
            m_RB.position = Vector3.MoveTowards(m_RB.position, m_nextLocation, moveSpeed);

            if (m_RB.position == m_nextLocation)
            {
                m_isMoving = false;
                m_playerPanelLocation += m_direction;
                Debug.Log("Now at panel: " + (m_playerPanelLocation + 1));
            }

        }
        #endregion
    }
}
