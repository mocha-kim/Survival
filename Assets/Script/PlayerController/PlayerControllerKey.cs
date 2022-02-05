using UnityEngine;

public class PlayerControllerKey : MonoBehaviour
{
    [SerializeField]
    private KeyCode     jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private CameraControllerKey cameraControllerKey;
    private Movement3DKey  movement3DKey;
    private Animator animator;

    private void Awake()
    {
        movement3DKey = GetComponent<Movement3DKey>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // x, z 방향 이동
        float x = Input.GetAxisRaw("Horizontal");   // 방향키 좌/우 움직임
        float z = Input.GetAxisRaw("Vertical");     // 방향키 위/아래 움직임

        movement3DKey.MoveTo(new Vector3(x, 0, z));

        // 점프키를 눌러 y축 방향으로 뛰어오르기 (Jump)
        if (Input.GetKeyDown(jumpKeyCode))
        {
            movement3DKey.JumpTo();
        }

        // Input.GetAxis("Mouse X") : 마우스를 좌/우로 움직였을 때
        // 왼쪽으로 이동 : -1 / 대기 : 0 / 오른쪽으로 이동 : 1
        // Input.GetAxis("Mouse Y") : 마우스를 위/아래로 움직였을 때
        // 아래로 이동 : -1 / 대기 : 0 / 위로 이동 : 1
        float mouseX = Input.GetAxis("Mouse X");    // 마우스 좌/우 움직임
        float mouseY = Input.GetAxis("Mouse Y");    // 마우스 위/아래 움직임

        cameraControllerKey.RotateTo(mouseX, mouseY);
    }
}
