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
        // ü���� ���ҵǰų� �ǰ� �ִϸ��̼��� ����Ǵ� ���� �ڵ带 �ۼ�
        Debug.Log(damage + "�� ü���� �����մϴ�.");

        // �ǰ� �ִϸ��̼� ���
        animator.SetTrigger("onHit");

    }

}
