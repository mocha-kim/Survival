using UnityEngine;

public class Movement3DKey : MonoBehaviour
{
    [SerializeField]
    private float   moveSpeed = 5.0f;   // 이동 속도
    [SerializeField]
    private float   jumpForce = 3.0f;   // 뛰는 힘
    [SerializeField]
    private float   gravity = -9.81f;   // 중력 계수
    private Vector3 moveDirection;      // 이동 방향

    [SerializeField]
    private Transform cameraTransform; // 카메라 Transform 컴포넌트
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // CharacterController.Move(Vector3 motion);
        // 매개변수로 이동방향, 속도, Time.deltaTime 등의 세부적인 이동 방법을 설정하면 이동을 수행한다.
        // CharacterController.SimpleMove(Vector3 speed);
        // 매개변수로 3 방향의 이동속도(=속력)를 넣어 호출하면 이동을 수행한다.
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // CharacterController.isGrounded
        // 발 위치의 충돌을 체크해 충돌이 되면 true, 충돌이 되지 않으면 false값을 나타내는 변수
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
    }

    public void MoveTo(Vector3 direction)
    {
        //moveDirection = direction;
        //moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
        Vector3 movedis = cameraTransform.rotation * direction;
        moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);
    }

    public void JumpTo()
    {
        if (characterController.isGrounded == true)
        {
            moveDirection.y = jumpForce;
        }
    }
}
