using UnityEngine;

/*
 * CameraController is player's camera manager
 * This manages camera's limit angle, move speed, target and distance
 * 
 * It can control camera's rotation with 'Right Mouse Click'
 * It can control camera's rotation with Keycode 'Q' & 'E'
 */

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform target; // Target (Player)
    [SerializeField]
    private float minDistance = 3; // Camera & Target distatnce (min)
    [SerializeField]
    private float maxDistance = 30; // Camera & Target distatnce (max)
    [SerializeField]
    private float wheelSpeed = 500; // Mouse wheel speed
    [SerializeField]
    private float xMoveSpeed = 500; // Camera Y speed
    [SerializeField]
    private float yMoveSpeed = 250; // Camera X speed
    private float yMinLimit = 5; // Camera X rotate limit (min)
    private float yMaxLimit = 80; // Camera X rotate limit (max)
    private float x, y; // Mouse movement value
    private float distance; // Camera & Target distance

    private void Awake()
    {
        // reset distance
        distance = Vector3.Distance(transform.position, target.position);

        // reset angles
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void Update()
    {
        if (target == null) return;

        // 1 : Right Mouse Click
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false; // Invisible mouse cursor
            //Cursor.lockState = CursorLockMode.Locked; // Locked mouse cursor

            // Mouse X & Mouse Y
            x += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;

            // Clamp Angle Y
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Camera Rotation
            transform.rotation = Quaternion.Euler(y, x, 0);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // KeyCode 'Q' Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            x -= 0.3f * xMoveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(y, x, 0);
        }

        // KeyCode 'E' Rotation
        if (Input.GetKey(KeyCode.E))
        {
            x += 0.3f * xMoveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(y, x, 0);
        }

        // Mouse wheel = distance
        distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;

        // Clamp distance
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Camera Position (+ distance)
        transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
