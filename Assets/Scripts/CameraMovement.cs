using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    private Vector3 m_Vel = Vector3.zero;
    private Vector3 m_Offset;

    public float m_KeepDistance = 3f;
    public float m_AbovePlayer = 3f;
    public float m_CamSpeed = 3f;

    private void Start()
    {
        m_Offset = (-target.forward * m_KeepDistance) + (target.up * m_AbovePlayer);
        gameObject.transform.position = target.position + m_Offset;
    }

    private void LateUpdate()
    {
        Vector3 nextPos = target.position + ((-target.forward * m_KeepDistance) + (target.up * m_AbovePlayer));
        transform.position = nextPos;


        transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
    }
}
