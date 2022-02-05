using UnityEngine;

public class Movement3D : MonoBehaviour
{

    [SerializeField]
    private float   moveSpeed = 5.0f;   // �̵� �ӵ�
    [SerializeField]
    private float   jumpForce = 3.0f;   // �ٴ� ��
    [SerializeField]
    private float   gravity = -9.81f;   // �߷� ���
    private Vector3 moveDirection;      // �̵� ����

    private CharacterController characterController;

    public float MoveSpeed
    {
        // �̵��ӵ��� 2 ~ 5 ������ ���� ���� ����
        set => moveSpeed = Mathf.Clamp(value, 2.0f, 5.0f);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �� ��ġ�� �浹�� üũ�� �浹�� �Ǹ� true, �浹�� ���� ������ false���� ��Ÿ���� ����
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        // �Ű������� 3 ������ �̵��ӵ�(=�ӷ�)�� �־� ȣ���ϸ� �̵��� �����Ѵ�.
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
