using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_GameManager;
    private GameObject m_PanelToMoveTo;
    private GameObject m_NextPanel;

    private Rigidbody m_RB;
    private Animator m_Animator;

    private Quaternion m_FaceDirection;

    [SerializeField] private float m_TurnSpeed;
    [SerializeField] private float m_MoveSpeed;
    private int m_CurrentPanel;
    private int m_Multiplier;

    public int multiplier
    {
        get { return m_Multiplier; }
        set { m_Multiplier = value; }
    }

    public int currentPanel
    {
        get { return m_CurrentPanel; }
    }

    private bool m_Moving;
    private bool m_Turning;

    public bool moving
    {
        get { return m_Moving; }
        set { m_Moving = value;  }
    }

    public bool turning
    {
        get { return m_Turning; }
        set { m_Turning = value; }
    }

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        //Setting up initial player values
        m_CurrentPanel = -1;
        
        m_Animator.SetBool("Moving", false);

        m_Moving = false;
        m_Turning = false;

        m_Multiplier = 1;
    }

    private void FixedUpdate()
    {
        if(m_Moving)
        {
            Move();
        }
        else if(m_Turning)
        {
            FacePanel(m_NextPanel);

            if(transform.rotation != m_FaceDirection)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_FaceDirection, m_TurnSpeed * Time.fixedDeltaTime);
            }
            else
            { 
                m_Turning = false;
            }
        }
    }

    public void Move()
    {

        FacePanel(m_PanelToMoveTo);        

        if (transform.rotation != m_FaceDirection)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_FaceDirection, m_TurnSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 nextPosition = new Vector3(m_PanelToMoveTo.transform.position.x, transform.position.y, m_PanelToMoveTo.transform.position.z);

            if (transform.position != nextPosition)
            {
                m_Animator.SetBool("Moving", true);
                m_RB.position = Vector3.MoveTowards(transform.position, nextPosition, m_MoveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (m_GameManager.GetComponent<GameManager>().GetDirection() == 1)
                {
                    m_CurrentPanel++;
                }
                else
                {
                    m_CurrentPanel--;
                }
                
                m_GameManager.GetComponent<GameManager>().MovedOneSpace();

                if (m_GameManager.GetComponent<GameManager>().GetSpacesToMove() == 0)
                {
                    m_Turning = true;
                    m_Animator.SetBool("Moving", false);
                }

                m_Moving = false;
            }
        }
        
    }

    public void SetPanelToGoTo(GameObject panel)
    {
        m_PanelToMoveTo = panel;
    }

    public void SetNextPanel(GameObject panel)
    {
        m_NextPanel = panel;
    }

    public void StopRunAnimaion()
    {
        m_Animator.SetBool("Moving", false);
    }

    public void StartVictoryAnimation()
    {
        m_Animator.SetTrigger("Won");
    }

    private void FacePanel(GameObject nextPanel)
    {
        Vector3 playerToNextPanel = Vector3.ProjectOnPlane(nextPanel.transform.position - transform.position, Vector3.up);

        m_FaceDirection = transform.rotation;

        if (playerToNextPanel != Vector3.zero)
        {
            m_FaceDirection = Quaternion.LookRotation(playerToNextPanel.normalized);
        }
    }
}


