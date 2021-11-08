using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;

    public Vector3 cameraOffset;
    private Vector3 vel = Vector3.zero;

    private void Start()
    {
        cameraOffset = new Vector3(-4, 12, 0);

        gameObject.transform.position = target.transform.position + cameraOffset;
    }

    private void LateUpdate()
    {
        gameObject.transform.position = target.transform.position + cameraOffset;
    }
}
