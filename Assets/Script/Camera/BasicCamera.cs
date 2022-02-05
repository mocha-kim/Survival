using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasicCamera : MonoBehaviour
{
    #region Variables

    public Vector3 offset;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;
    public float distance;

    private float x;
    private float y;
    public float xRotSpeed = 10.0f;
    public float yRotSpeed = 10.0f;

    public Transform target;

    public LayerMask obstacleMask;

    private Vector3 refVelocity;

    #endregion Variables

    private void Start()
    {
        offset = offset.normalized;
        transform.position = target.position + offset * distance;
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    public void HandleCamera()
    {
        if (!target) return;

        if (Input.GetMouseButton(1))
        {
            x = Input.GetAxis("Mouse X") * xRotSpeed;
            y = Input.GetAxis("Mouse Y") * yRotSpeed;

            transform.RotateAround(target.transform.position, Vector3.up, x);
            transform.RotateAround(target.transform.position, Vector3.left, y);

            offset = (transform.position - target.position).normalized;
        }

        Vector3 calcPosition = target.position + offset * distance;
        float clampY = Mathf.Clamp(calcPosition.y, 1, 5);
        Vector3 finalPosition = new(calcPosition.x, clampY, calcPosition.z);

        Vector3 lookAtPosition = target.position;
        lookAtPosition.y += lookAtHeight;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(lookAtPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if (target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
