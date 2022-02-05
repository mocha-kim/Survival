using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private Transform cameraTransform;
    private Movement3D movement3D;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        // x, z 방향 이동
        float x = Input.GetAxisRaw("Horizontal");   // 방향키 좌/우 움직임
        float z = Input.GetAxisRaw("Vertical");     // 방향키 위/아래 움직임

        // 애니메이션 파라미터 설정 (horizontal, vertical)
        playerAnimator.OnMovement(x, z);

        // 이동 속도 설정 (앞으로 이동할때만 5, 나머지는 2)
        movement3D.MoveSpeed = z > 0 ? 5.0f : 2.0f;

        // 이동 함수 호출 (카메라가 보고있는 방향을 기준으로 방향키에 따라 이동)
        movement3D.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));

        // 회전 설정 (항상 앞만 보도록 캐릭터의 회전은 카메라와 같은 회전 값으로 설정)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // 점프키를 눌러 y축 방향으로 뛰어오르기 (Jump)
        if (Input.GetKeyDown(jumpKeyCode))
        {
            playerAnimator.OnJump(); // 애니메이션 파라미터 (onJump)
            movement3D.JumpTo(); // 점프 함수 호출
        }

        // 마우스 왼쪽 버튼을 누르면 무기 공격
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.OnWeaponAttack();
        }
    }

}
