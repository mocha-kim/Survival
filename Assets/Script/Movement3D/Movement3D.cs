using UnityEngine;

public class Movement3D : MonoBehaviour
{

    [SerializeField]
    private float   moveSpeed = 5.0f;   // 이동 속도
    [SerializeField]
    private float   jumpForce = 3.0f;   // 뛰는 힘
    [SerializeField]
    private float   gravity = -9.81f;   // 중력 계수
    private Vector3 moveDirection;      // 이동 방향

    private CharacterController characterController;

    public float MoveSpeed
    {
        // 이동속도는 2 ~ 5 사이의 값만 설정 가능
        set => moveSpeed = Mathf.Clamp(value, 2.0f, 5.0f);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 발 위치의 충돌을 체크해 충돌이 되면 true, 충돌이 되지 않으면 false값을 나타내는 변수
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        // 매개변수로 3 방향의 이동속도(=속력)를 넣어 호출하면 이동을 수행한다.
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        //moveDirection = direction;
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
        //Vector3 movedis = cameraTransform.rotation * direction;
        //moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);
    }

    public void JumpTo()
    {
        if (characterController.isGrounded == true)
        {
            moveDirection.y = jumpForce;
        }
    }

}
