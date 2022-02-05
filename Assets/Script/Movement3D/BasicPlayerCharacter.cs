using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerCharacter : MonoBehaviour
{
    #region Variables

    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;

    public float gravity = -9.81f;
    public Vector3 drags;

    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private bool isGrounded = false;

    private CharacterController characterController;
    private Vector3 inputDirection = Vector3.zero;

    private Vector3 calcVelocity;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && calcVelocity.y < 0)
            calcVelocity.y = 0;

        // Get move inputs
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * speed);
        if (move != Vector3.zero)
            transform.forward = move;

        // Get jump inputs
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        // Calc gravity
        calcVelocity.y += gravity * Time.deltaTime;

        // Calc dash drags
        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;

        characterController.Move(calcVelocity * Time.deltaTime);
    }
}
