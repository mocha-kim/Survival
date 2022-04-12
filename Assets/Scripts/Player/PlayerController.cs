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
    private DoubleKeyPress[] keys;
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
    private bool walk = false;
    private bool run = false;
    [SerializeField]
    private float crouchTime;
    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float runTime;

    // Game system objects
    private StatsObject playerStat;
    private InventoryObject inventory;
    private InventoryObject quickslot;

    // Attributes
    private float con;
    private float str;
    private float def;
    private float handiness;
    private float cooking;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Start()
    {
        playerStat = GameManager.Instance.playerStats;
        inventory = GameManager.Instance.inventory;
        quickslot = GameManager.Instance.quickslot;

        inventory.OnUseItem += OnUseItem;
        quickslot.OnUseItem += OnUseItem;

        playerStat.OnStatChanged += OnStatChanged;
        QuestManager.Instance.OnRewardedQuest += OnRewardedQuest;

        keys = new[]
        {
        new DoubleKeyPress(KeyCode.W),
        new DoubleKeyPress(KeyCode.A),
        new DoubleKeyPress(KeyCode.S),
        new DoubleKeyPress(KeyCode.D),
        };
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying && !playerStat.IsDead)
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
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, mask))
                {
                    isGround = true;
                }
            }

            // Jump : Press jumpKeyCode & Player is grounded
            if (Input.GetKeyDown(jumpKeyCode) && isGround)
            {
                moveDirection.y = jumpForce;
                playerAnimator.OnJump();
                playerStat.AddStatusCurrentValue(StatusType.SP, -1);
                playerStat.AddAttributeExp(AttributeType.CON, 1);
            }

            // Move : moveSpeed(0, 1.0f, 2.0f, 5.0f = idle, crouch, walk, run)
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.z = Input.GetAxisRaw("Vertical");

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].Update();
            }
            keys[0].UpdateAction(() => walk = true, () => run = true);
            keys[1].UpdateAction(() => walk = true, () => run = true);
            keys[2].UpdateAction(() => walk = true, () => run = true);
            keys[3].UpdateAction(() => walk = true, () => run = true);

            if (Input.GetKey(crouchKeyCode))
            {
                moveSpeed = 1.0f;
                crouchTime += Time.deltaTime;
                playerStat.AddStatusCurrentValue(StatusType.SP, -0.003f);
                if(crouchTime > 10f)
                {
                    playerStat.AddAttributeExp(AttributeType.CON, 3);
                    crouchTime = 0;
                }
            }
            else if (run == true)
            {
                moveSpeed = 5.0f;
                runTime += Time.deltaTime;
                playerStat.AddStatusCurrentValue(StatusType.SP, -0.005f);
                if (runTime > 10f)
                {
                    playerStat.AddAttributeExp(AttributeType.CON, 5);
                    runTime = 0;
                }
            }
            else if (walk == true)
            {
                moveSpeed = 2.0f;
                walkTime += Time.deltaTime;
                playerStat.AddStatusCurrentValue(StatusType.SP, -0.001f);
                if (walkTime > 10f)
                {
                    playerStat.AddAttributeExp(AttributeType.CON, 1);
                    walkTime = 0;
                }
            }
            else
            {
                crouchTime = 0;
                walkTime = 0;
                runTime = 0;
                moveSpeed = 0;
            }

            Vector3 movement;
            isMove = !(moveDirection.x == 0 && moveDirection.z == 0);
            if (isMove) // Player Move (x, y, z)
            {
                // cameraTransform.(x, y, z) = cameraTransform.(right, up, forward)
                Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;
                Vector3 lookDir = (lookForward * moveDirection.z + lookRight * moveDirection.x) * moveSpeed;

                // Player Rotation
                Quaternion rotate = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 10);

                movement = new Vector3(lookDir.x, moveDirection.y, lookDir.z);
            }
            else // Player Move (0, y, 0)
            {
                movement = new Vector3(0, moveDirection.y, 0);
                walk = false;
                run = false;
            }
            characterController.Move(movement * Time.deltaTime);

            // Animation
            playerAnimator.OnMove(isMove, moveSpeed);

            // Attack : left control key & isDelay = false
            if (Input.GetKeyDown(attackKeyCode) && !isDelay)
            {
                isDelay = true;
                playerAnimator.OnWeaponAttack();
                playerStat.AddStatusCurrentValue(StatusType.SP, -1);
                StartCoroutine(Delay(attackDelay));
            }
        }
    }

    public void OnHit(float damage)
    {
        playerStat.AddStatusCurrentValue(StatusType.HP, -damage * def);
        playerStat.AddAttributeExp(AttributeType.DEF, 5);

        if (playerStat.IsDead)
        {
            playerAnimator.Dead();
        }
        else
        {
            playerAnimator.OnHit();
        }
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        isDelay = false;
    }

    public void OnStatChanged(StatsObject stats)
    {
        /*
        private float con;
        private float str;
        private float def;
        private float handiness;
        private float cooking;
        */

        // AttackCollision manage (str = stats.attributes[AttributeType.STR].modifiedValue;)
        def = 100f / (100 + stats.attributes[AttributeType.DEF].modifiedValue);
    }

    private void OnUseItem(ItemObject item)
    {
        foreach (ItemEffect effect in item.data.effects)
        {
            switch (effect.type)
            {
                case EffectType.Status:
                    foreach (Status status in playerStat.statuses.Values)
                    {
                        if (status.type == effect.statusType)
                        {
                            playerStat.AddStatusCurrentValue(effect.statusType, (int)effect.value);
                            break;
                        }
                    }
                    break;
                case EffectType.Attribute:
                    foreach (Attribute attribute in playerStat.attributes.Values)
                    {
                        if (attribute.type == effect.attributeType)
                        {
                            playerStat.AddAttributeExp(effect.attributeType, effect.value);
                            break;
                        }
                    }
                    break;
                case EffectType.Condition:
                    foreach (Condition condition in playerStat.conditions.Values)
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

    private void OnRewardedQuest(QuestObject quest)
    {
        switch (quest.camp)
        {
            case CampType.Lawful:
                playerStat.lawfulCoin += quest.data.rewardGold;
                break;
            case CampType.Neutral:
                playerStat.neutralCoin += quest.data.rewardGold;
                break;
            case CampType.Chaotic:
                playerStat.chaoticCoin += quest.data.rewardGold;
                break;
        }
    }

}
