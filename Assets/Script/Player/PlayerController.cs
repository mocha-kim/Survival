using UnityEngine;

/*
 * PlayerController is player's main script
 * 
 * This should fix the jump and gravity.
 */

public class PlayerController : MonoBehaviour
{
    // Component
    [SerializeField]
    private StatsObject playerStat;
    [SerializeField]
    private Transform cameraTransform;
    private CharacterController characterController;
    private PlayerAnimator playerAnimator;

    // Movement Control
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode runKeyCode = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode crouchKeyCode = KeyCode.LeftControl;
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    private float gravity = -9.8f;
    private Vector3 moveDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    public void Start()
    {
        //Debug.Log("stats : " + playerStat.statuses[(int)StatusType.HP].Percentage);
    }

    private void Update()
    {
        Gravity();
        
        Jump();
        
        Move();

        Attack();
    }

    private void Gravity()
    {
        // Player is not grounded
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        // Press jumpKeyCode & Player is grounded
        if (Input.GetKeyDown(jumpKeyCode)) //  && characterController.isGrounded == true
        {
            moveDirection.y = jumpForce;
            playerAnimator.OnJump();
        }
    }

    private void Move()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");

        // isMove : Check player movement
        bool isMove = !(moveDirection.x == 0 && moveDirection.z == 0);

        if (Input.GetKey(runKeyCode))         moveSpeed = 5.0f; // Run
        else if (Input.GetKey(crouchKeyCode)) moveSpeed = 1.0f; // Crouch
        else                                  moveSpeed = 2.0f; // Walk

        if (isMove)
        {
            // cameraTransform.(x, y, z) = cameraTransform.(right, up, forward)
            // lookForward : Normalized Vertical (vector size = 1)
            // lookRight   : Normalized Horizontal (vector size = 1)
            // movement    : Normalized Movement (size : horizontal = Vertical = diagonal)
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;
            Vector3 lookDir = (lookForward * moveDirection.z + lookRight * moveDirection.x) * moveSpeed;

            // Player Rotation
            // 1. transform.forward = lookForward : can't see player's face
            // 2. transform.forward = movement    : can see player's face
            transform.forward = lookDir;

            // Player Move (x, y, z)
            //transform.position += new Vector3(lookDir.x, moveDirection.y, lookDir.z) * Time.deltaTime;
            characterController.Move(new Vector3(lookDir.x, moveDirection.y, lookDir.z) * Time.deltaTime);
        }
        else
        {
            // Player Move (0, y, 0)
            //transform.position += moveDirection * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        }

        // Animation
        playerAnimator.OnMove(isMove, moveSpeed);
    }

    private void Attack()
    {
        // 0 : Left Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.OnWeaponAttack();
        }
    }

    public void OnHit(int damage)
    {
        Debug.Log("Player takes " + damage + " Damage!!");

        // Animation
        playerAnimator.OnHit();
    }
}
