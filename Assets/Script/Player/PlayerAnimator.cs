using UnityEngine;

/*
 * PlayerAnimator is player's animation
 * This manages move(walk, run, crouch), jump and attack animation
 * And gives the enemy a hit signal
 */

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField]
    private GameObject attackCollision;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnJump()
    {
        animator.SetTrigger("onJump");
    }

    public void OnWeaponAttack()
    {
        animator.SetTrigger("onWeaponAttack");
    }

    // Animator Events
    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void OnMove(bool isWalk, float speed)
    {
        animator.SetBool("isWalk", isWalk);
        
        if (speed == 5.0f)
        {
            animator.SetBool("isRun", true);
        }
        else if (speed == 1.0f)
        {
            animator.SetBool("isCrouch", true);
        }
        else
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isCrouch", false);
        }
    }

    public void OnHit()
    {
        animator.SetTrigger("onHit");
    }
}
