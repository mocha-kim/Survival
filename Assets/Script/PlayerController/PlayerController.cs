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
        // x, z ���� �̵�
        float x = Input.GetAxisRaw("Horizontal");   // ����Ű ��/�� ������
        float z = Input.GetAxisRaw("Vertical");     // ����Ű ��/�Ʒ� ������

        // �ִϸ��̼� �Ķ���� ���� (horizontal, vertical)
        playerAnimator.OnMovement(x, z);

        // �̵� �ӵ� ���� (������ �̵��Ҷ��� 5, �������� 2)
        movement3D.MoveSpeed = z > 0 ? 5.0f : 2.0f;

        // �̵� �Լ� ȣ�� (ī�޶� �����ִ� ������ �������� ����Ű�� ���� �̵�)
        movement3D.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));

        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // ����Ű�� ���� y�� �������� �پ������ (Jump)
        if (Input.GetKeyDown(jumpKeyCode))
        {
            playerAnimator.OnJump(); // �ִϸ��̼� �Ķ���� (onJump)
            movement3D.JumpTo(); // ���� �Լ� ȣ��
        }

        // ���콺 ���� ��ư�� ������ ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.OnWeaponAttack();
        }
    }

}
