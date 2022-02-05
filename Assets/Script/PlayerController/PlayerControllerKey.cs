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
        // x, z ���� �̵�
        float x = Input.GetAxisRaw("Horizontal");   // ����Ű ��/�� ������
        float z = Input.GetAxisRaw("Vertical");     // ����Ű ��/�Ʒ� ������

        movement3DKey.MoveTo(new Vector3(x, 0, z));

        // ����Ű�� ���� y�� �������� �پ������ (Jump)
        if (Input.GetKeyDown(jumpKeyCode))
        {
            movement3DKey.JumpTo();
        }

        // Input.GetAxis("Mouse X") : ���콺�� ��/��� �������� ��
        // �������� �̵� : -1 / ��� : 0 / ���������� �̵� : 1
        // Input.GetAxis("Mouse Y") : ���콺�� ��/�Ʒ��� �������� ��
        // �Ʒ��� �̵� : -1 / ��� : 0 / ���� �̵� : 1
        float mouseX = Input.GetAxis("Mouse X");    // ���콺 ��/�� ������
        float mouseY = Input.GetAxis("Mouse Y");    // ���콺 ��/�Ʒ� ������

        cameraControllerKey.RotateTo(mouseX, mouseY);
    }
}
