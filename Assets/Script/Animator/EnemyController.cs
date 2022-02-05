using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        // 체력이 감소되거나 피격 애니메이션이 재생되는 등의 코드를 작성
        Debug.Log(damage + "의 체력이 감소합니다.");

        // 피격 애니메이션 재생
        animator.SetTrigger("onHit");

    }

}
