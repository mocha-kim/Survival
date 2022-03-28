using System.Collections;
using UnityEngine;

/*
 * PlayerController is player's main script
 * 
 */

public class PlayerController : MonoBehaviour
{
    // Component
    [SerializeField]
    private Transform cameraTransform;
    private CharacterController characterController;
    private PlayerAnimator playerAnimator;

    // Key Setting
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private KeyCode crouchKeyCode = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode attackKeyCode = KeyCode.LeftControl;

    // Movement Control
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    private float gravity = -9.8f;
    private Vector3 moveDirection;
    public LayerMask mask;

    // System Setting
    private bool isGround;
    private bool isDelay;
    private bool isMove;
    private float attackDelay = 1.1335f;

    // Game system objects
    [SerializeField]
    private StatsObject playerStat;
    [SerializeField]
    private InventoryObject inventory;
    [SerializeField]
    private InventoryObject quickslot;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Start()
    {
        inventory.OnUseItem += OnUseItem;
        quickslot.OnUseItem += OnUseItem;

        foreach (QuestObject quest in QuestManager.Instance.acceptedQuests.questObjects)
        {
            if (quest.type == QuestType.AcquireItem)
            {
                int count = 0;
                if (inventory.IsContain(quest.data.targetID))
                {
                    count += inventory.CountItem(quest.data.targetID);
                }
                if (quickslot.IsContain(quest.data.targetID))
                {
                    count += quickslot.CountItem(quest.data.targetID);
                }
                QuestManager.Instance.SetQuestCurValue(quest, count);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying)
        {
            // Gravity : Check player is not grounded
            if (characterController.isGrounded == false)
            {
                moveDirection.y += gravity * Time.deltaTime;
            }

            // Ground Check
            isGround = false;
            if (Mathf.Abs(characterController.velocity.y) < jumpForce / 3f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, mask))
                {
                    isGround = true;
                }
            }

            // Jump : Press jumpKeyCode & Player is grounded
            if (Input.GetKeyDown(jumpKeyCode) && isGround)
            {
                moveDirection.y = jumpForce;
                playerAnimator.OnJump();
            }

            // Move
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.z = Input.GetAxisRaw("Vertical");

            if (Input.GetMouseButton(0))
            {
                moveSpeed = 5.0f; // Run
            }
            else if (Input.GetKey(crouchKeyCode))
            {
                moveSpeed = 1.0f; // Crouch
            }
            else
            {
                moveSpeed = 2.0f; // Walk
            }

            // isMove : Check player movement
            isMove = !(moveDirection.x == 0 && moveDirection.z == 0);

            if (isMove)
            {
                // cameraTransform.(x, y, z) = cameraTransform.(right, up, forward)
                Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;
                Vector3 lookDir = (lookForward * moveDirection.z + lookRight * moveDirection.x) * moveSpeed;

                // Player Rotation
                transform.forward = lookDir;

                // Player Move (x, y, z)
                characterController.Move(new Vector3(lookDir.x, moveDirection.y, lookDir.z) * Time.deltaTime);
            }
            else
            {
                // Player Move (0, y, 0)
                characterController.Move(moveDirection * Time.deltaTime);
            }

            // Animation
            playerAnimator.OnMove(isMove, moveSpeed);

            // Attack : left control key & isDelay = false
            if (Input.GetKeyDown(attackKeyCode) && !isDelay)
            {
                isDelay = true;
                playerAnimator.OnWeaponAttack();
                StartCoroutine(Delay(attackDelay));
            }
        }
    }

    public void OnHit(int damage)
    {
        Debug.Log("Player takes " + damage + " Damage!!");

        // Animation
        playerAnimator.OnHit();
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        isDelay = false;
    }

    private void OnUseItem(ItemObject item)
    {
        foreach (ItemEffect effect in item.data.effects)
        {
            switch (effect.type)
            {
                case EffectType.Status:
                    foreach (Status status in playerStat.statuses)
                    {
                        if (status.type == effect.statusType)
                        {
                            playerStat.AddStatusValue(effect.statusType, (int)effect.value);
                            break;
                        }
                    }
                    break;
                case EffectType.Attribute:
                    foreach (Attribute attribute in playerStat.attributes)
                    {
                        if (attribute.type == effect.attributeType)
                        {
                            playerStat.AddAttributeExp(effect.attributeType, effect.value);
                            break;
                        }
                    }
                    break;
                case EffectType.Condition:
                    foreach (Condition condition in playerStat.conditions)
                    {
                        if (condition.type == effect.conditionType)
                        {
                            playerStat.ActivateCondition(effect.conditionType, effect.value);
                            break;
                        }
                    }
                    break;
            }
        }
    }
}