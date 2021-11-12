using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    private Vector3 m_Vel = Vector3.zero;

    public float m_KeepDistance = 5f;
    public float m_AbovePlayer = 20f;
    public float m_CamSpeed = 3f;

    private void Start()
    {

        gameObject.transform.position = new Vector3(target.position.x, transform.position.y + m_AbovePlayer, transform.position.z - m_KeepDistance);
    }

    private void LateUpdate()
    {
        //gameObject.transform.position = new Vector3(target.position.x, transform.position.y + 10f, transform.position.z - m_KeepDistance);

        Vector3 nextCamPos = new Vector3(target.position.x, target.position.y + m_AbovePlayer, target.position.z - m_KeepDistance);

        transform.position = nextCamPos;


    }
}
